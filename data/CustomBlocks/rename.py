import os
import re

folder = "C:/Users/tobia/GitProjekte/AutoAlteration/data/CustomBlocks/HeavySurface"

for root, _, files in os.walk(folder):
    for file in files:
        if file.endswith(".Item.Gbx"):
            path = os.path.join(root, file)
            new_path = path.replace("-HeavyWood-", "")
            os.rename(os.path.abspath(path), os.path.abspath(new_path))
