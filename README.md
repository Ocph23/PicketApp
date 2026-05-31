# PicketApp Deployment

## Files

- `docker-compose.prod.yml` - production Compose file for Debian
- `.env.production.example` - template for production environment variables
- `deploy.sh` - helper script to build and start the stack
- `build-and-push.sh` - helper script to build and push images to a registry
- `nginx/prod.conf` - reverse proxy config for the domain

## Debian Deployment

1. Copy the environment template.

```bash
cp .env.production.example .env.production
```

2. Edit `.env.production` and set strong values for:

- `POSTGRES_PASSWORD`
- `REDIS_PASSWORD`
- `JWT_SECRET`
- `PUBLIC_DOMAIN`
- `APP_VERSION`

3. Run the deploy script.

```bash
chmod +x deploy.sh
./deploy.sh
```

If you prefer manual commands:

```bash
docker compose --env-file .env.production -f docker-compose.prod.yml pull db cache
docker compose --env-file .env.production -f docker-compose.prod.yml up -d
```

## Build and Push Images

From your development machine:

```bash
./build-and-push.sh yourdockerhubuser 1.0.0
```

That script builds and pushes:

- `yourdockerhubuser/piketapi:1.0.0`
- `yourdockerhubuser/picket-client:1.0.0`
- `yourdockerhubuser/whatsapp:1.0.0`

Then update `.env.production` on the Debian server to match those image names.

## Reverse Proxy

Nginx listens on port `80` and routes:

- `/` to `admin-client`
- `/api/` to `piketapi`
- `/whatsapp/` to `whatsapp`

Set `PUBLIC_DOMAIN` in `.env.production` to your real domain name.

The frontend now uses runtime config:

- if `VITE_API_URL` is set, it is used
- otherwise it falls back to `window.location.origin`
- the API calls then use `${origin}/api`

## Services

- `piketapi` on port `5001`
- `admin-client` on port `8000`
- `whatsapp` on port `3000`

## Notes

- `postgres_data` persists the database
- `whatsapp_auth` persists WhatsApp session data
- `db` and `cache` are not exposed publicly in production
