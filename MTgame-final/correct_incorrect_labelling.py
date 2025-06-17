# a script to label correct & incorrect selections 

import os
import pandas as pd

# Define the folder paths
folders = [
    "ERPLogsAutumn", 
    "ERPLogsSummer",
    "ERPLogsWinter", 
    "ERPLogsStandard"
]

# Load correct items
correct_items_path = "correct_items.csv"
correct_items_df = pd.read_csv(correct_items_path, header=None)
correct_items = set(correct_items_df.iloc[:, 0].astype(str))

# Loop through each folder
for folder in folders:
    labeled_folder = os.path.join(f'{folder}_labeled')
    os.makedirs(labeled_folder, exist_ok=True)

    for filename in os.listdir(folder):
        if filename.endswith('.csv'):
            file_path = os.path.join(folder, filename)

            # Read the file without headers
            df = pd.read_csv(file_path, header=None)

            # Use the first row as header & get rid of empty spaces 
            df.columns = df.iloc[0].astype(str).str.strip()
            df = df[1:]  # Drop the header row

            # Label selections as correct (1) or incorrect (0)
            df['correct'] = df['ObjectName'].apply(lambda x: 1 if str(x).strip() in correct_items else 0)

            # Save cleaned version
            cleaned_path = os.path.join(labeled_folder, filename)
            df.to_csv(cleaned_path, index=False)

print("All files with now labeled selections saved to corresponding 'XXX_labeled' folders.")
