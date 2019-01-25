import sqlite3
import random
import string

connection = sqlite3.connect('GAIA.db')
cursor = connection.cursor()

cursor.execute('CREATE TABLE IF NOT EXISTS stars (designation TEXT PRIMARY KEY, x REAL, y REAL, z REAL, colour INTEGER, brightness REAL, sort_id INTEGER);')

for i in range(20000000):
    #### TEST VALUES ####
    designation = str(i) 
    x = random.random()
    y = random.random()
    z = random.random()
    colour = 8107013
    brightness = random.random()
    sort_id = 0;
    #### ---------- ####
    
    cursor.execute('INSERT INTO stars(designation, x, y, z, colour, brightness) VALUES(?, ?, ?, ?, ?, ?)', (designation, x, y, z, colour, brightness))

connection.commit()
