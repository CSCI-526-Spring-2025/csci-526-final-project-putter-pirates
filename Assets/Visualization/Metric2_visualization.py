from collections import defaultdict
import matplotlib.pyplot as plt
import requests
import matplotlib as mpl

# Initialize Firebase URL for level0 completed analytics
levelNum = 3
DATABASE_URL = f"https://putterdatabase-default-rtdb.firebaseio.com/analytics/level{levelNum}/completed.json"
response = requests.get(DATABASE_URL)

if response.status_code == 200:
    data = response.json()

    # Aggregation dictionaries
    state_count = defaultdict(int)
    state_shots = defaultdict(int)

    # Aggregate data over each game entry
    for game_id, game_data in data.items():
        shots = game_data.get('shots', 0)
        state = game_data.get('state', {})  # Assuming state is iterable (like a list)
        
        # Generate a key from state values, e.g., "0_2"
        state_order = '_'.join(str(item) for item in state)
        
        state_count[state_order] += 1
        state_shots[state_order] += shots

    # Prepare pie chart data
    labels = list(state_count.keys())
    sizes = [state_count[label] for label in labels]

    # Calculate average shots per state_order
    average_shots = []
    for label in labels:
        count = state_count[label]
        total = state_shots[label]
        avg = total / count if count != 0 else 0
        average_shots.append(avg)

    # Normalize average shots for color mapping using the Reds colormap
    norm = plt.Normalize(min(average_shots), max(average_shots))
    colors = [plt.cm.Reds(norm(val)) for val in average_shots]

    # Plot pie chart with figure and axes background set to 'lightyellow'
    plt.figure(figsize=(8, 8), facecolor='white')
    ax = plt.gca()
    ax.set_facecolor('white')

    # Draw the pie chart and capture returned objects
    wedges, texts, autotexts = plt.pie(
        sizes, labels=labels, colors=colors, startangle=90, autopct='%1.1f%%',
        wedgeprops={'edgecolor': 'black', 'linewidth': 1}
    )

    plt.title(f'Level {levelNum} - State Order Usage with Color Gradient by Average Shot', color='black', fontweight='bold')
    
    # Update each slice's autotext to include the average shot value
    for i, autotext in enumerate(autotexts):
        current_text = autotext.get_text()  # e.g., "25.0%"
        autotext.set_text(f"{current_text}\n(Avg: {average_shots[i]:.1f})")

    plt.axis('equal')
    
    # Create a ScalarMappable for the colorbar using the same normalization and colormap
    sm = mpl.cm.ScalarMappable(norm=norm, cmap=plt.cm.Reds)
    sm.set_array([])

    # Add a colorbar (legend) for average shot values; attach it to the current Axes
    plt.colorbar(sm, ax=ax, orientation='vertical', pad=0.1, label="Average Shot")
    
    plt.show()
else:
    print("Failed to fetch data:", response.status_code)
