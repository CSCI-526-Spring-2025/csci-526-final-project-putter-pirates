import requests
import pandas as pd
import matplotlib.pyplot as plt

# Firebase Realtime Database URL (Replace with your actual Firebase URL)
DATABASE_URL = "https://putterdatabase-default-rtdb.firebaseio.com/analytics.json"

# Fetch data from Firebase
response = requests.get(DATABASE_URL)

if response.status_code == 200:
    data = response.json()

    # Dictionaries to store level-wise average aim assist and complete aim assist lists
    level_avg_aim = {}
    level_aim_all = {}

    # Loop through levels in the data
    for level, details in data.items():
        if "aim" in details:
            # Assuming details["aim"] is a dictionary of entries
            aim_list = [entry.get("aim", 0) for entry in details["aim"].values()]
            if aim_list:
                level_avg_aim[level] = sum(aim_list) / len(aim_list)
            else:
                level_avg_aim[level] = 0
            # Save the full aim list for box plot visualization
            level_aim_all[level] = aim_list

    # (Optional) Create a DataFrame with average aim values
    df = pd.DataFrame(list(level_avg_aim.items()), columns=["Level", "Average Aim Assist"])
    print(df)

    # Sort level keys assuming level names are like "level0", "level1", etc.
    sorted_levels = sorted(level_avg_aim.keys(), key=lambda x: int(x.replace("level", "")))
    sorted_avg_aim = [level_avg_aim[level] for level in sorted_levels]
    # For the box plot, prepare the aim distribution lists in sorted order
    box_data = [level_aim_all[level] for level in sorted_levels]

    # Create a figure with two vertically stacked subplots
    fig, axs = plt.subplots(2, 1, figsize=(10, 10))

    # Plot Bar Chart: Average Aim Assist per Level
    axs[0].bar(sorted_levels, sorted_avg_aim, alpha=0.75, color='skyblue')
    axs[0].set_xlabel("Levels")
    axs[0].set_ylabel("Average Aim Assist")
    axs[0].set_title("Average Aim Assist per Level")
    axs[0].grid(axis="y", linestyle="--", alpha=0.7)

    # Plot Box Plot: Distribution of Aim Assist Values per Level
    axs[1].boxplot(box_data, labels=sorted_levels)
    axs[1].set_xlabel("Levels")
    axs[1].set_ylabel("Aim Assist")
    axs[1].set_title("Aim Assist Distribution per Level")
    axs[1].grid(axis="y", linestyle="--", alpha=0.7)

    plt.tight_layout()
    plt.show()

else:
    print("Failed to fetch data:", response.status_code)
