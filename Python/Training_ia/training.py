from transformers import ViTImageProcessor
from PIL import Image

from datasets import load_dataset

"""model_name_or_path = 'google/vit-base-patch16-224-in21k'
processor = ViTImageProcessor.from_pretrained(model_name_or_path)

image = 'C:/Users/jerem/OneDrive/Documents/SmartCar/Python/Training_ia/test.jpg'  # Replace with your image path

image = Image.open(image).convert('RGB')  # Open the image and convert to RGB

image_tensor = processor(image, return_tensors='pt')
print(image_tensor)"""


ds = load_dataset('beans')
print(ds)