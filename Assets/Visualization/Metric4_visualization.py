import requests
import matplotlib.pyplot as plt

# Replace with your Firebase URL (must end with .json to get JSON)
DATABASE_URL = "https://putterdatabase-default-rtdb.firebaseio.com/analytics.json"

response = requests.get(DATABASE_URL)
if response.status_code == 200:
    data = response.json()
    
    # Lists for levels 2-7 and 8-12
    levels_2_7 = []
    help_values_2_7 = []
    levels_8_12 = []
    help_values_8_12 = []
    
    # Traverse each level entry under analytics
    for level_name, level_data in data.items():
        # Parse the numeric part of the level (assumes keys like "level2", "level3", etc.)
        try:
            level_num = int("".join(filter(str.isdigit, level_name)))
        except ValueError:
            continue  # Skip if no numeric part is found
        
        # Safely get the 'help' value (default to 0 if not found)
        help_val = level_data.get('help', 0)
        
        # Group levels based on their numeric value
        if 2 <= level_num <= 7:
            levels_2_7.append(level_name)
            help_values_2_7.append(help_val)
        elif 8 <= level_num <= 12:
            levels_8_12.append(level_name)
            help_values_8_12.append(help_val)
    
    # Sort the levels in each group based on the numeric part for correct order
    sorted_2_7 = sorted(zip(levels_2_7, help_values_2_7), key=lambda x: int("".join(filter(str.isdigit, x[0]))))
    sorted_8_12 = sorted(zip(levels_8_12, help_values_8_12), key=lambda x: int("".join(filter(str.isdigit, x[0]))))
    
    if sorted_2_7:
        levels_2_7, help_values_2_7 = zip(*sorted_2_7)
    else:
        levels_2_7, help_values_2_7 = [], []
    
    if sorted_8_12:
        levels_8_12, help_values_8_12 = zip(*sorted_8_12)
    else:
        levels_8_12, help_values_8_12 = [], []
    
    # Create a figure with two subplots (vertically stacked)
    fig, axs = plt.subplots(2, 1, figsize=(10, 10))
    
    # Plot for levels 2-7
    axs[0].bar(levels_2_7, help_values_2_7, color='teal')
    axs[0].set_xlabel("Levels 2-7")
    axs[0].set_ylabel("Help Value")
    axs[0].set_title("Help used in Wind levels")
    axs[0].tick_params(axis="x", rotation=45)
    axs[0].grid(axis="y", linestyle="--", alpha=0.7)
    
    # Plot for levels 8-12
    axs[1].bar(levels_8_12, help_values_8_12, color='teal')
    axs[1].set_xlabel("Levels 8-12")
    axs[1].set_ylabel("Help Value")
    axs[1].set_title("Help used in Electricity levels")
    axs[1].tick_params(axis="x", rotation=45)
    axs[1].grid(axis="y", linestyle="--", alpha=0.7)
    
    plt.tight_layout()
    plt.show()
    
else:
    print("Failed to fetch data. Status code:", response.status_code)
