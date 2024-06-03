import discord
from discord.ext import commands, tasks
import requests
from bs4 import BeautifulSoup
import datetime
import json
import os
import asyncio
from dotenv import load_dotenv

# Laad de omgevingsvariabelen uit het .env bestand
load_dotenv()
TOKEN = os.getenv('DISCORD_TOKEN')

# Bestand om de kanaalinstellingen op te slaan
SETTINGS_FILE = 'settings.json'

# URL van de Wikipedia-pagina
WIKIPEDIA_URL = 'https://nl.wikipedia.org/wiki/Hoofdpagina'

# Functie om het gewenste gedeelte van de Wikipedia-pagina op te halen
def get_wist_je_dat():
    response = requests.get(WIKIPEDIA_URL)
    soup = BeautifulSoup(response.text, 'html.parser')
    wist_je_dat_div = soup.find('div', {'id': 'mp-itn'})
    wist_je_dat_text = wist_je_dat_div.get_text() if wist_je_dat_div else "Kon het gedeelte niet vinden op de Wikipedia-pagina."
    return wist_je_dat_text

# Functie om instellingen te laden
def load_settings():
    if os.path.exists(SETTINGS_FILE) and os.path.isfile(SETTINGS_FILE):
        with open(SETTINGS_FILE, 'r') as f:
            try:
                return json.load(f)
            except json.JSONDecodeError:
                return {}
    return {}

# Functie om instellingen op te slaan
def save_settings(settings):
    with open(SETTINGS_FILE, 'w') as f:
        json.dump(settings, f, indent=4)

# Discord client initialisatie met berichtinhoudsintentie
intents = discord.Intents.default()
intents.guilds = True
intents.messages = True
intents.message_content = True
bot = commands.Bot(command_prefix='/', intents=intents)
settings = load_settings()

# Event handler voor het opstarten van de bot
@bot.event
async def on_ready():
    print(f'We have logged in as {bot.user}')
    print("Bot is klaar voor gebruik!")

    # Checken of er een kanaal is ingesteld via het setchannel-commando
    if not settings:
        for guild in bot.guilds:
            settings[str(guild.id)] = guild.system_channel.id if guild.system_channel else None
        save_settings(settings)

    # Planning van de taak om dagelijks om half 9 's ochtends het gedeelte op te halen en te posten
    while True:
        now = datetime.datetime.now()
        next_run = now.replace(hour=8, minute=30, second=0, microsecond=0)
        if now >= next_run:
            next_run += datetime.timedelta(days=1)
        await asyncio.sleep((next_run - now).total_seconds())

        wist_je_dat_text = get_wist_je_dat()
        for guild_id, channel_id in settings.items():
            if channel_id:
                channel = bot.get_channel(channel_id)
                if channel:
                    await channel.send(wist_je_dat_text)

# Event handler voor berichten
@bot.event
async def on_message(message):
    if message.author == bot.user:
        return
    
    if bot.user.mentioned_in(message):
        if 'members' in message.content.lower():
            await message.channel.send(f"This server has {message.guild.member_count} members")
        elif 'setchannel' in message.content.lower():
            parts = message.content.split()
            if len(parts) >= 3:
                channel_id = int(parts[2].strip('<#>'))
                settings[str(message.guild.id)] = channel_id
                save_settings(settings)
                channel = bot.get_channel(channel_id)
                if channel:
                    await message.channel.send(f'Wist-je-dat kanaal ingesteld op {channel.name}')
                else:
                    await message.channel.send('Ongeldig kanaal ID.')
            else:
                await message.channel.send('Gebruik: @bot setchannel <channel_id>')
        elif 'sync' in message.content.lower():
            if message.author.id == 154640915917570058:
                await bot.tree.sync()
                await message.channel.send('Slash commando\'s zijn gesynchroniseerd.')
                print('Slash commando\'s zijn gesynchroniseerd.')
            else:
                await message.channel.send('You must be the owner to use this command!')
        elif 'test_wistjedat' in message.content.lower():
            wist_je_dat_text = get_wist_je_dat()
            await message.channel.send(wist_je_dat_text)
        else:
            embed = get_help_embed()
            await message.channel.send(embed=embed)
    await bot.process_commands(message)

# Functie voor het maken van het help embed
def get_help_embed():
    embed = discord.Embed(title="Help")
    embed.add_field(name="setchannel", value="Stel het kanaal in voor het ontvangen van Wist-je-dat berichten", inline=False)
    embed.add_field(name="members", value="Geeft het aantal leden in de server weer", inline=False)
    embed.add_field(name="sync", value="Synchroniseer de slash commando's (alleen eigenaar)", inline=False)
    embed.add_field(name="test_wistjedat", value="Test het Wist-je-dat bericht handmatig", inline=False)
    return embed

# Start de Discord-bot
bot.run(TOKEN)
