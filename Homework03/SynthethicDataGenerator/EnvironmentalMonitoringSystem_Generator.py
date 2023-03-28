import random
import json
from datetime import datetime, timedelta

# Define start and end dates
start_date = datetime(2023, 4, 1)
end_date = datetime(2023, 4, 3)

# Define temperature range in Celsius
temp_range = (0, 15)

# Define humidity range in percentage
humidity_range = (60, 90)

# Define CO2 level range in ppm
co2_range = (840, 860)

# Define particulate matter range in micrograms per cubic meter
pm_range = (10, 15)

# Define soil moisture range in percentage
soil_moisture_range = (30, 40)

# Define light level range in lux
light_range = (800, 1000)

# Define wind speed range in meters per second
wind_speed_range = (3, 6)

# Define wind direction range in degrees
wind_direction_range = (100, 140)

# Define precipitation range in millimeters
precipitation_range = (0, 2)

# Define pH level range for water quality
ph_range = (6.8, 7.5)

# Define dissolved oxygen range for water quality in mg/L
do_range = (8, 9)

# Define noise level range in decibels
noise_range = (30, 40)

# Create empty list to store IoT data
iot_data = []

# Generate IoT data for each 5 minute interval
delta = timedelta(minutes=5)
current_date = start_date
while current_date < end_date:
    # Generate random values for each environmental parameter
    temperature = round(random.uniform(*temp_range), 1)
    humidity = round(random.uniform(*humidity_range), 1)
    co2_level = random.randint(*co2_range)
    particulate_matter = random.randint(*pm_range)
    soil_moisture = round(random.uniform(*soil_moisture_range), 1)
    light_level = random.randint(*light_range)
    wind_speed = round(random.uniform(*wind_speed_range), 1)
    wind_direction = random.randint(*wind_direction_range)
    precipitation = round(random.uniform(*precipitation_range), 1)
    ph_level = round(random.uniform(*ph_range), 1)
    dissolved_oxygen = round(random.uniform(*do_range), 1)
    noise_level = random.randint(*noise_range)

    # Create a dictionary for the current IoT data point
    data_point = {
        "timestamp": current_date.strftime("%Y-%m-%dT%H:%M:%SZ"),
        "temperature": temperature,
        "humidity": humidity,
        "co2_level": co2_level,
        "particulate_matter": particulate_matter,
        "soil_moisture": soil_moisture,
        "light_level": light_level,
        "wind_speed": wind_speed,
        "wind_direction": wind_direction,
        "precipitation": precipitation,
        "water_quality": {
            "ph_level": ph_level,
            "dissolved_oxygen": dissolved_oxygen
        },
        "noise_level": noise_level
    }

    # Add the current IoT data point to the list
    iot_data.append(data_point)

    # Increment the current date
    current_date += delta

# Convert IoT data to JSON and print it
print(json.dumps(iot_data))
