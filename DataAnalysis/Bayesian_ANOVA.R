
### This is some code for performing a Bayesian 2x3 repeated measures ANOVA 
### The factors and their interaction to be analyzed are condition (gamified vs. standard) and scene (1 vs. 2 vs. 3)
### The dependent variable is BCI task accuracy 


# import packages
library(tidyverse)
library(ggpubr)
library(BayesFactor)
library(tidyr)
library(rstatix)


# set working directory
setwd("C:/Users/jette/Documents/Uni/Tilburg/Semester4_Thesis/Game Data")

# set seed for reproducibility 
set.seed(42)

# read in the ANOVA-readied data 
acc_data <- read.csv("anova_structure_acc.csv")
acc_data

# convert scene and condition into factor variables 
acc_data <- acc_data %>% convert_as_factor(scene, condition, ID)

# get some summary stats
acc_data %>% group_by(condition, scene) %>% get_summary_stats(SceneAccuracy, type = 'mean_sd')

# look at factor levels and structure 
str(acc_data)
levels(acc_data$condition)
levels(acc_data$scene)

# check for outliers 
acc_data %>% group_by(condition, scene) %>% identify_outliers(SceneAccuracy)
#acc_data

# qq plots for visual assessment - for post-hoc tests later
ggqqplot(acc_data, "SceneAccuracy", ggtheme = theme_classic()) + 
  facet_grid(scene ~ condition, labeller = "label_both")

# compute the bayesian ANOVA 
bf <- anovaBF(SceneAccuracy ~ condition*scene + ID, data = acc_data, whichRandom = "ID")
bf

# plot the Bayes factors 
plot(bf)

# Residuals from frequentist model -- this is only a proxy 
# tests in JASP show that the quantiles of the observed residuals from Bayesian model are normally distributed 
resids <- residuals(lm(SceneAccuracy ~ condition * scene + ID, data = acc_data))
ggqqplot(resids, ggtheme = theme_classic()) + ggtitle("QQ Plot of Residuals")

# post hoc bayesian paired t-tests
# restructure data for post hoc tests 
# Combine condition and scene into a single within-subject factor
acc_data$cond_scene <- interaction(acc_data$condition, acc_data$scene)

# Pivot to wide format
wide_data <- acc_data %>%
  select(ID, cond_scene, SceneAccuracy) %>%
  pivot_wider(names_from = cond_scene, values_from = SceneAccuracy)

wide_data

## pairwise comparison of conditions within each scene 
# scene 1: g vs. s 
ph_condition_1 <- ttestBF(x = wide_data$gamified.1, y = wide_data$standard.1, paired = TRUE)  
ph_condition_1
# get posterior samples 
posterior_1 <- posterior(ph_condition_1, iterations = 10000)
cat("Posterior mean difference (mu):", round(mean(posterior_1[, "mu"]), 3), "\n")
cat("95% Credible Interval for mu:", quantile(posterior_1[, "mu"], c(0.025, 0.975)), "\n")

# scene 2: g vs. s
ph_condition_2 <- ttestBF(x = wide_data$gamified.2, y = wide_data$standard.2, paired = TRUE)
ph_condition_2
# get posterior samples 
posterior_2 <- posterior(ph_condition_2, iterations = 10000)
cat("Posterior mean difference (mu):", round(mean(posterior_2[, "mu"]), 3), "\n")
cat("95% Credible Interval for mu:", quantile(posterior_2[, "mu"], c(0.025, 0.975)), "\n")

# scene 3: g vs. s
ph_condition_3 <- ttestBF(x = wide_data$gamified.3, y = wide_data$standard.3, paired = TRUE)
ph_condition_3
# get posterior samples 
posterior_3 <- posterior(ph_condition_3, iterations = 10000)
cat("Posterior mean difference (mu):", round(mean(posterior_3[, "mu"]), 3), "\n")
cat("95% Credible Interval for mu:", quantile(posterior_3[, "mu"], c(0.025, 0.975)), "\n")


## pairwise comparison of scenes within each condition 
# gamified: scene 1 vs. scene 2 
ph_scene_g12 <- ttestBF(x = wide_data$gamified.1, y = wide_data$gamified.2, paired = TRUE)
ph_scene_g12
# get posterior samples 
posterior_g12 <- posterior(ph_scene_g12, iterations = 10000)
cat("Posterior mean difference (mu):", round(mean(posterior_g12[, "mu"]), 3), "\n")
cat("95% Credible Interval for mu:", quantile(posterior_g12[, "mu"], c(0.025, 0.975)), "\n")

# gamified: scene 1 vs. scene 3
ph_scene_g13 <- ttestBF(x = wide_data$gamified.1, y = wide_data$gamified.3, paired = TRUE)
ph_scene_g13
# get posterior samples 
posterior_g13 <- posterior(ph_scene_g13, iterations = 10000)
cat("Posterior mean difference (mu):", round(mean(posterior_g13[, "mu"]), 3), "\n")
cat("95% Credible Interval for mu:", quantile(posterior_g13[, "mu"], c(0.025, 0.975)), "\n")

# gamified: scene 2 vs. scene 3
ph_scene_g23 <- ttestBF(x = wide_data$gamified.2, y = wide_data$gamified.3, paired = TRUE)
ph_scene_g23
# get posterior samples 
posterior_g23 <- posterior(ph_scene_g23, iterations = 10000)
cat("Posterior mean difference (mu):", round(mean(posterior_g23[, "mu"]), 3), "\n")
cat("95% Credible Interval for mu:", quantile(posterior_g23[, "mu"], c(0.025, 0.975)), "\n")

# standard: scene 1 vs. scene 2 
ph_scene_s12 <- ttestBF(x = wide_data$standard.1, y = wide_data$standard.2, paired = TRUE)
ph_scene_s12
# get posterior samples 
posterior_s12 <- posterior(ph_scene_s12, iterations = 10000)
cat("Posterior mean difference (mu):", round(mean(posterior_s12[, "mu"]), 3), "\n")
cat("95% Credible Interval for mu:", quantile(posterior_s12[, "mu"], c(0.025, 0.975)), "\n")

# standard: scene 1 vs. scene 3
ph_scene_s13 <- ttestBF(x = wide_data$standard.1, y = wide_data$standard.3, paired = TRUE)
ph_scene_s13
# get posterior samples 
posterior_s13 <- posterior(ph_scene_s13, iterations = 10000)
cat("Posterior mean difference (mu):", round(mean(posterior_s13[, "mu"]), 3), "\n")
cat("95% Credible Interval for mu:", quantile(posterior_s13[, "mu"], c(0.025, 0.975)), "\n")

# standard: scene 2 vs. scene 3 
ph_scene_s23 <- ttestBF(x = wide_data$standard.2, y = wide_data$standard.3, paired = TRUE)
ph_scene_s23
# get posterior samples 
posterior_s23 <- posterior(ph_scene_s23, iterations = 10000)
cat("Posterior mean difference (mu):", round(mean(posterior_s23[, "mu"]), 3), "\n")
cat("95% Credible Interval for mu:", quantile(posterior_s23[, "mu"], c(0.025, 0.975)), "\n")



