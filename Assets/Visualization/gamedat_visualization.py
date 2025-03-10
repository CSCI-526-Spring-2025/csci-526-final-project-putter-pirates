import requests
import pandas as pd
import matplotlib.pyplot as plt

# Firebase Realtime Database URL (Replace with your actual Firebase URL)
DATABASE_URL = "https://putterdatabase-default-rtdb.firebaseio.com/analytics.json"

# Fetch data from Firebase
response = requests.get(DATABASE_URL)

if response.status_code == 200:
    data = response.json()

    # Dictionary to store level-wise shot averages
    level_shots = {}

    # Loop through levels
    for level, details in data.items():
        if "shots" in details:
            shots_list = [entry.get("shots", 0) for entry in details["shots"].values()]
            level_shots[level] = sum(shots_list) / len(shots_list) if shots_list else 0  # Calculate average

    # Convert to DataFrame
    df = pd.DataFrame([level_shots])

    # Print DataFrame
    print(df)

    # Plot bar chart
    plt.figure(figsize=(8, 5))
    plt.bar(level_shots.keys(), level_shots.values(), alpha=0.75, color='skyblue')
    plt.xlabel("Levels")
    plt.ylabel("Average Shots")
    plt.title("Average Shots per Level")
    plt.xticks(rotation=45)
    plt.grid(axis="y", linestyle="--", alpha=0.7)

    # Show the plot
    plt.show()


else:
    print("Failed to fetch data:", response.status_code)
