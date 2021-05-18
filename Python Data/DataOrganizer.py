import time
import numpy as np
import matplotlib.pyplot as plt  # 3.4.2

data = open("Assets\data.txt", "r").readlines()
UPDATE = int(data[0][:-1])

"""
File Format:
Time between each recorded point
Time since startup
Organism Number
Food Number
"""

while True:
    data = open("Assets\data.txt", "r").readlines()

    total_time = float(data[1][:-1])
    organisms = int(data[2][:-1])
    food = int(data[3][:-1])

    # *old version
    # print(f"Time: {total_time}\nOrganism #: {organisms}\nFood #: {food}\n")

    # time.sleep(update * 10)
    
    # *matplotlib version
    plt.scatter(total_time, food, c='#0000ff')
    plt.scatter(total_time, organisms, c='#ff0000')
    plt.pause(UPDATE)

plt.show()