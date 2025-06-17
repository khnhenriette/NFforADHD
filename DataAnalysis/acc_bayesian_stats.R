
### An R script for the statistical analysis of the BCI task accuracy data 
### Rather than using frequentist / inferential statistics a Bayesian t-test will be used 

# import libraries
library(BayesFactor)

# set working directory
setwd("C:/Users/jette/Documents/Uni/Tilburg/Semester4_Thesis/Game Data")

# set seed for reproducibility 
set.seed(42)

# read in data 
acc_data <- read.csv("acc_diffs.csv")
acc_data

standard <- acc_data$Accuracy_standard 
standard

game <- acc_data$Accuracy_game
game

## Investigate accuracy difference between standard and gamified condition 
# run Bayesian paired t-test 
bf_acc <- ttestBF(x = game, y = standard, paired = TRUE)
print(bf_acc)

# get posterior samples 
posterior_acc <- posterior(bf_acc, iterations = 10000)
summary(posterior_acc)

cat("Posterior mean difference (mu):", round(mean(posterior_acc[, "mu"]), 3), "\n")
cat("95% Credible Interval for mu:", quantile(posterior_acc[, "mu"], c(0.025, 0.975)), "\n")

# start saving plot
png(filename = 'accuracy_posterior.png', width = 1800, height = 2400, res = 300)

# plot the posterior distributions 
plot(posterior_acc, parameter = "mu") 

# Finish saving plot
dev.off()








