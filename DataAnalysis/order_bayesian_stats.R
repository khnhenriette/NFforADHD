
### An R script for the statistical analysis of the BCI task accuracy data 
### Investigate accuracy difference between first and second task (independent of whether standard or gamified)
### I.e. investigate novelty effect 
### Rather than using frequentist / inferential statistics a Bayesian t-test will be used 

# import libraries
library(BayesFactor)

# set working directory
setwd("C:/Users/jette/Documents/Uni/Tilburg/Semester4_Thesis/Game Data")

# set seed for reproducibility 
set.seed(42)

# read in data 
order_data <- read.csv("order_diffs.csv")
order_data

one <- order_data$Accuracy_1 
one

two <- order_data$Accuracy_2
two

## Investigate accuracy difference between standard and gamified condition 
# run Bayesian paired t-test 
bf_order <- ttestBF(x = one, y = two, paired = TRUE)
print(bf_order)

# get posterior samples 
posterior_order <- posterior(bf_order, iterations = 10000)
summary(posterior_order)

cat("Posterior mean difference (mu):", round(mean(posterior_order[, "mu"]), 3), "\n")
cat("95% Credible Interval for mu:", quantile(posterior_order[, "mu"], c(0.025, 0.975)), "\n")

# start saving plot
png(filename = 'order_posterior.png', width = 1800, height = 2400, res = 300)

# plot the posterior distributions 
plot(posterior_order, parameter = "mu") 

# Finish saving plot
dev.off()




