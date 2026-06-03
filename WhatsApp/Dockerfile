FROM node:20-slim

WORKDIR /usr/src/app

ENV NODE_ENV=production \
    CHROME_BIN=/usr/bin/chromium \
    WWEBJS_AUTH_PATH=/usr/src/app/.wwebjs_auth \
    PUPPETEER_SKIP_CHROMIUM_DOWNLOAD=true

RUN apt-get update && apt-get install -y --no-install-recommends \
    ca-certificates \
    chromium \
    libasound2 \
    libatk-bridge2.0-0 \
    libatk1.0-0 \
    libc6 \
    libcairo2 \
    libcups2 \
    libdbus-1-3 \
    libexpat1 \
    libfontconfig1 \
    libgcc1 \
    libglib2.0-0 \
    libgtk-3-0 \
    libnspr4 \
    libnss3 \
    libpango-1.0-0 \
    libpangocairo-1.0-0 \
    libstdc++6 \
    libx11-6 \
    libx11-xcb1 \
    libxcb1 \
    libxcomposite1 \
    libxcursor1 \
    libxdamage1 \
    libxext6 \
    libxfixes3 \
    libxi6 \
    libxrandr2 \
    libxrender1 \
    libxss1 \
    libxtst6 \
    fonts-liberation \
    libappindicator1 \
    libgbm1 \
    lsb-release \
    xdg-utils \
    && rm -rf /var/lib/apt/lists/*

COPY package*.json ./

RUN npm ci --omit=dev --ignore-scripts

COPY . .

RUN mkdir -p /usr/src/app/.wwebjs_auth
RUN useradd -m -u 10001 app \
    && chown -R app:app /usr/src/app

USER app

EXPOSE 3000

CMD ["npm", "start"]
