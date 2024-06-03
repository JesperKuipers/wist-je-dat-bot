# Gebaseerd op de officiële Python image
FROM python:3.8-slim-buster

# Werkomgeving instellen
WORKDIR /app

# Vereisten kopiëren en installeren
COPY requirements.txt requirements.txt
RUN pip install --no-cache-dir -r requirements.txt

# Kopieer de rest van de applicatiecode
COPY . .

# De bot starten
CMD ["python", "bot.py"]
