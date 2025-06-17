
### An R script for the statistical analysis of the questionnaire data 
### Rather than using frequentist / inferential statistics (t-test or Wilcoxon), here their Bayesian counterparts will be used 
### The data for the Core & Post-Game module is both processed here 

# import libraries
library(BayesFactor)
library(DFBA)

# set working directory
setwd("C:/Users/jette/Documents/Uni/Tilburg/Semester4_Thesis/Game Data")

# set seed for reproducibility 
set.seed(42)

# read in data
q_data <- read.csv("full_experience.csv")

# Create folder if it doesn't exist
dir.create("Component_Posteriors", showWarnings = FALSE)

# names of the questionnaire components to extract 
vars <- c("Competence", "Immersion", "Flow", "Tension", 
          "Challenge", "NegAff", "PosAff", "PosExp", 
          "NegExp", "Tiredness", "Returning")


# define function to check normality using Shapiro-Wilk test
is_normal <- function(x) {
  shapiro.test(x)$p.value > 0.05
}


# loop over all the components / variables 
for (var in vars) {
  # separate standard and game data 
  s_vals <- q_data[q_data$condition == "s", var]
  g_vals <- q_data[q_data$condition == "g", var]
  
  # calculate paired differences 
  diffs <- g_vals - s_vals 
  
  cat("\n### Variable:", var, "###\n")
  
  # filename for saving posterior plot
  file_name <- paste0("Component_Posteriors/", var, "_posterior.png")
  
  # start saving plot
  png(filename = file_name, width = 1800, height = 2400, res = 300)
  
  if (is_normal(diffs)) {
    cat("Data seems to be normally distributed: Running Bayesian paired t-test \n")
    
    # run Bayesian paired t-test
    bf <- ttestBF(x = g_vals, y = s_vals, paired = TRUE) # to test g > s
    #bf <- ttestBF(x = s_vals, y = g_vals, paired = TRUE)  # to test s > g
    print(bf)
    
     # get posterior samples 
    post <- posterior(bf, iterations = 10000)
    print(summary(post))
    
    cat("Posterior mean difference (mu):", round(mean(post[, "mu"]), 3), "\n")
    cat("95% Credible Interval for mu:", quantile(post[, "mu"], c(0.025, 0.975)), "\n")
    
    # plot posterior for mean difference
    plot(post, parameter = "mu")
    
  } else {
    cat("Data does not seem to be normally distributed: Running Bayesian Wilcoxon signed-rank test \n")
    
    wilc <- dfba_wilcoxon(Y1 = g_vals, Y2 = s_vals)  # to test g > s
    #wilc <- dfba_wilcoxon(Y1 = s_vals, Y2 = g_vals)   # to test s > g 
    print(wilc)
    
    # plot posterior
    plot(wilc)

  }
  
  # finish saving plot
  dev.off()
  
}

