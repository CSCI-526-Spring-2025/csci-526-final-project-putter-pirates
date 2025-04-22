from collections import defaultdict
import matplotlib.pyplot as plt
import requests
import matplotlib as mpl

# --- Configuration ---
levelNum = 1
DATABASE_URL = f"https://putterdatabase-default-rtdb.firebaseio.com/analytics/level{levelNum}/completed.json"
response = requests.get(DATABASE_URL)

if response.status_code == 200:
    data = response.json()

    # --- Aggregation ---
    state_count = defaultdict(int)
    state_shots = defaultdict(int)

    for game_data in data.values():
        shots = game_data.get('shots', 0)
        state = game_data.get('state', [])
        state_order = '_'.join(str(s) for s in state)
        state_count[state_order] += 1
        state_shots[state_order] += shots

    # --- Data prep ---
    labels = list(state_count.keys())
    sizes = [state_count[label] for label in labels]
    average_shots = [state_shots[label] / state_count[label] for label in labels]

    # Sort by count
    sorted_indices = sorted(range(len(sizes)), key=lambda i: sizes[i], reverse=True)
    labels = [labels[i] for i in sorted_indices]
    sizes = [sizes[i] for i in sorted_indices]
    average_shots = [average_shots[i] for i in sorted_indices]

    # Normalize for color
    norm = plt.Normalize(min(average_shots), max(average_shots))
    colors = [plt.cm.Reds(norm(val)) for val in average_shots]

    # --- Plot Bar Chart ---
    fig, ax = plt.subplots(figsize=(10, 6), facecolor='white')
    bars = ax.barh(labels, sizes, color=colors, edgecolor='black')

    ax.set_title(f'Level {levelNum} - State Order Usage\n(Colored by Avg Shot)', fontsize=14, fontweight='bold')
    ax.set_xlabel('Number of Occurrences')
    ax.set_ylabel('State Order')

    # Annotate each bar with avg shot
    for i, bar in enumerate(bars):
        ax.text(bar.get_width() + 0.5, bar.get_y() + bar.get_height() / 2,
                f"Avg: {average_shots[i]:.1f}", va='center', fontsize=9)

    # Colorbar to show avg shot gradient
    sm = mpl.cm.ScalarMappable(norm=norm, cmap=plt.cm.Reds)
    sm.set_array([])
    cbar = plt.colorbar(sm, ax=ax, orientation='vertical')
    cbar.set_label("Average Shot", fontsize=10)

    plt.tight_layout()
    plt.show()

else:
    print("Failed to fetch data:", response.status_code)
