# NFforADHD
The repository includes:
- the unity project folder for a gamified NF task
- the unity project folder for a standard NF task
- code for the analysis of BCI task accuracy and user experience data:
  * R: acc_bayesian_stats for a Bayesian t-test comparing accuracy in the gamified against the standard condition
  * R: Bayesian_ANOVA for a Bayesian 2x3 repeated measures ANOVA for a more detailed analysis of the differences in accuracy between the gamified and standard condition
  * R: order_bayesian_stats for a Bayesian t-test to check for a novelty effect
  * R: questionnaire_bayesian_stats for a Bayesian analysis of differences in the user experience for the gamified and the standard condition
  * JASP: Multinomial_preference for a Bayesion analysis of the preferences for either, both, or neither condition 
  * python: preparationStatisticsAccData, preparationStatisticsGEQData, preprocessGameData, preprocessStandardData for the preprocessing and some EDA & plotting of the accuracy and experience data 

(Note: the raw data for this project is not included in the public repository for data protection reasons) 

User experience data was obtained using the game experience questionnaire (see IJsselsteijn, W. A., De Kort, Y. A., & Poels, K. (2013). The game experience questionnaire.)
