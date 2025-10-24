import os

def flattenFolderStructure(folder):
    for root, _, files in os.walk(folder):
        for file in files:
            if file.endswith(".Item.Gbx") or file.endswith(".Block.Gbx"):
                path = os.path.join(root, file)
                new_path = os.path.join(folder, file).replace("-HeavyWood-", "").replace("-HeavyWood", "")
                os.rename(os.path.abspath(path), os.path.abspath(new_path))

if __name__ == "__main__":
    flattenFolderStructure("C:/Users/Tobias/Documents/Programmieren/AutoAlteration/data/CustomBlocks/HeavySurface")