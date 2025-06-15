from transformers import ViTImageProcessor
from PIL import Image
from datasets import load_dataset, ClassLabel
import torch
import numpy as np
from evaluate import load
from transformers import ViTForImageClassification, TrainingArguments, Trainer


model_name_or_path = 'google/vit-base-patch16-224-in21k'
processor = ViTImageProcessor.from_pretrained(model_name_or_path)
metric = load("accuracy")

def compute_metrics(p):
    return metric.compute(predictions=np.argmax(p.predictions, axis=1), references=p.label_ids)


def collate_fn(batch):
    print("batch", batch[0])
    return {
        'pixel_values': torch.stack([x['pixel_values'] for x in batch]),
        'labels': torch.tensor([x['labels'] for x in batch])
    }

def transform(batch):
    inputs = processor([x for x in batch['image']], return_tensors='pt')
    inputs['labels'] = batch['labels']
    return inputs

ds = load_dataset('imagefolder', data_dir='C:/Users/jerem/OneDrive/Documents/SmartCar/Python/Training_ia/dataset')
labels = ClassLabel(num_classes=2, names=[0, 1]).names

prepared_ds = ds.with_transform(transform)

model = ViTForImageClassification.from_pretrained(
    model_name_or_path,
    num_labels=len(labels),
    id2label={str(i): c for i, c in enumerate(labels)},
    label2id={c: str(i) for i, c in enumerate(labels)}
)

training_args = TrainingArguments(
  output_dir="./SmartCar_ViT",
  overwrite_output_dir=True,
  per_device_train_batch_size=16,
  eval_strategy="steps",
  num_train_epochs=4,
  fp16=True,
  save_steps=100,
  eval_steps=100,
  logging_steps=10,
  learning_rate=2e-4,
  save_total_limit=2,
  remove_unused_columns=False,
  push_to_hub=False,
  load_best_model_at_end=True,
)

trainer = Trainer(
    model=model,
    args=training_args,
    data_collator=collate_fn,
    compute_metrics=compute_metrics,
    train_dataset=prepared_ds["train"],
    eval_dataset=prepared_ds["validation"],
    tokenizer=processor,
)

train_results = trainer.train()
trainer.save_model()
trainer.log_metrics("train", train_results.metrics)
trainer.save_metrics("train", train_results.metrics)
trainer.save_state()

metrics = trainer.evaluate(prepared_ds['validation'])
trainer.log_metrics("eval", metrics)
trainer.save_metrics("eval", metrics)