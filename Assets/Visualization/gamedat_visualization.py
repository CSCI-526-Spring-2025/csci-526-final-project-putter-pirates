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


else:
    print("Failed to fetch data:", response.status_code)
