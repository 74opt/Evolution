import time
import numpy as np
import matplotlib.pyplot as plt  # 3.4.2

data = open("Assets\data.txt", "r").readlines()
update = int(data[0][:-2])

"""
File Format:
Time between each recorded point
Time since startup
Organism Number
Food Number
"""

while True:
    data = open("Assets\data.txt", "r").readlines()

    total_time = int(data[1][:-1])
    organisms = int(data[2][:-1])
    food = int(data[3][:-1])

    print(f"Time: {total_time}\nOrganism #: {organisms}\nFood #: {food}\n")

    time.sleep(update * 10)
    