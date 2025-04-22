import requests
import matplotlib.pyplot as plt
import matplotlib.cm as cm
import numpy as np
import seaborn as sns

# Apply Seaborn style
sns.set(style="whitegrid")

DATABASE_URL = "https://putterdatabase-default-rtdb.firebaseio.com/analytics.json"
response = requests.get(DATABASE_URL)

if response.status_code == 200:
    data = response.json()
    
    levels_2_7, help_values_2_7 = [], []
    levels_8_12, help_values_8_12 = [], []

    for level_name, level_data in data.items():
        try:
            level_num = int("".join(filter(str.isdigit, level_name)))
        except ValueError:
            continue

        help_val = level_data.get('help', 0)

        if 2 <= level_num <= 7:
            levels_2_7.append(level_name)
            help_values_2_7.append(help_val)
        elif 8 <= level_num <= 12:
            levels_8_12.append(level_name)
            help_values_8_12.append(help_val)

    sorted_2_7 = sorted(zip(levels_2_7, help_values_2_7), key=lambda x: int("".join(filter(str.isdigit, x[0]))))
    sorted_8_12 = sorted(zip(levels_8_12, help_values_8_12), key=lambda x: int("".join(filter(str.isdigit, x[0]))))

    levels_2_7, help_values_2_7 = zip(*sorted_2_7) if sorted_2_7 else ([], [])
    levels_8_12, help_values_8_12 = zip(*sorted_8_12) if sorted_8_12 else ([], [])

    fig, axs = plt.subplots(2, 1, figsize=(12, 9))
    plt.subplots_adjust(hspace=0.4)

    def get_blue_shades(values):
        norm = plt.Normalize(min(values), max(values))
        return cm.Blues(norm(values))  # Shades of blue

    def add_value_labels(ax, bars):
        for bar in bars:
            height = bar.get_height()
            if height > 0:
                ax.annotate(f'{height}',
                            xy=(bar.get_x() + bar.get_width() / 2, height),
                            xytext=(0, -12),
                            textcoords="offset points",
                            ha='center', va='bottom',
                            color='white', fontsize=10, fontweight='bold')

    # Wind levels
    colors_2_7 = get_blue_shades(np.array(help_values_2_7))
    bars1 = axs[0].bar(levels_2_7, help_values_2_7, color=colors_2_7, width=0.5)
    axs[0].set_title("Help Usage in Wind Levels", fontsize=14, fontweight='bold')
    axs[0].set_ylabel("Help Value")
    axs[0].tick_params(axis='x', rotation=45)
    add_value_labels(axs[0], bars1)

    # Electricity levels
    colors_8_12 = get_blue_shades(np.array(help_values_8_12))
    bars2 = axs[1].bar(levels_8_12, help_values_8_12, color=colors_8_12, width=0.5)
    axs[1].set_title("Help Usage in Electricity Levels", fontsize=14, fontweight='bold')
    axs[1].set_ylabel("Help Value")
    axs[1].tick_params(axis='x', rotation=45)
    add_value_labels(axs[1], bars2)

    plt.suptitle("Help Usage Analysis by Level", fontsize=16, fontweight='bold')
    plt.tight_layout(rect=[0, 0, 1, 0.96])
    plt.show()

else:
    print("Failed to fetch data. Status code:", response.status_code)