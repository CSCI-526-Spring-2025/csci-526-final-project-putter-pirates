import requests
import pandas as pd
import matplotlib.pyplot as plt

# Firebase Realtime Database URL (Replace with your actual Firebase URL)
DATABASE_URL = "https://putterdatabase-default-rtdb.firebaseio.com/analytics.json"

# Fetch data from Firebase
response = requests.get(DATABASE_URL)
if response.status_code == 200:
    data = response.json()

    # Dictionaries to store level-wise average shots and the complete shots list
    level_avg_shots = {}
    level_shots_all = {}

    # Loop through levels in the fetched data
    for level, details in data.items():
        if level == "level13":
            continue
        if "shots" in details:
            # Assume details["shots"] is a dictionary of entries; extract shots counts
            shots_list = [entry.get("shots", 0) for entry in details["shots"].values()]
            level_shots_all[level] = shots_list
            # Calculate average shots for the level
            level_avg_shots[level] = sum(shots_list) / len(shots_list) if shots_list else 0

    # Create a DataFrame of average shots for debugging/logging purposes
    df = pd.DataFrame(list(level_avg_shots.items()), columns=["Level", "Average Shots"])
    print(df)

    # Sort levels so they appear in order. Here, we assume level names are like "level0", "level1", etc.
    sorted_levels = sorted(level_avg_shots.keys(), key=lambda x: int(x.replace("level", "")))
    sorted_avg_shots = [level_avg_shots[level] for level in sorted_levels]
    # Prepare box plot data: list of shots lists in the sorted order
    box_data = [level_shots_all[level] for level in sorted_levels]

    # Create a figure with two vertically stacked subplots
    fig, axs = plt.subplots(2, 1, figsize=(10, 10))

    # Bar Chart: Average Shots per Level
    axs[0].bar(sorted_levels, sorted_avg_shots, alpha=0.75, color='skyblue')
    axs[0].set_xlabel("Levels")
    axs[0].set_ylabel("Average Shots")
    axs[0].set_title("Average Shots per Level")
    axs[0].grid(axis="y", linestyle="--", alpha=0.7)

    # Box Plot: Distribution of Shots per Level
    axs[1].boxplot(box_data, labels=sorted_levels)
    axs[1].set_xlabel("Levels")
    axs[1].set_ylabel("Shots Distribution")
    axs[1].set_title("Shots Distribution per Level")
    axs[1].grid(axis="y", linestyle="--", alpha=0.7)

    plt.tight_layout()
    plt.show()

else:
    print("Failed to fetch data:", response.status_code)
