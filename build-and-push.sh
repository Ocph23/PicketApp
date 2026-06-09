#!/usr/bin/env bash
set -euo pipefail

REGISTRY_PREFIX="${1:-}"
TAG="${2:-latest}"

if [ -z "$REGISTRY_PREFIX" ]; then
  echo "Usage: ./build-and-push.sh <registry-prefix> [tag]" >&2
  echo "Example: ./build-and-push.sh yourdockerhubuser latest" >&2
  exit 1
fi

if ! command -v docker >/dev/null 2>&1; then
  echo "docker is not installed or not in PATH" >&2
  exit 1
fi

PIKETAPI_IMAGE="${REGISTRY_PREFIX}/piketapi:${TAG}"
ADMIN_CLIENT_IMAGE="${REGISTRY_PREFIX}/picket-client:${TAG}"
WHATSAPP_IMAGE="${REGISTRY_PREFIX}/whatsapp:${TAG}"

echo "Building $PIKETAPI_IMAGE"
docker build -t "$PIKETAPI_IMAGE" -f PiketWebApi/Dockerfile .

echo "Building $ADMIN_CLIENT_IMAGE"
docker build -t "$ADMIN_CLIENT_IMAGE" -f smkn8picket-client/Dockerfile ./smkn8picket-client

echo "Pushing $PIKETAPI_IMAGE"
docker push "$PIKETAPI_IMAGE"

echo "Pushing $ADMIN_CLIENT_IMAGE"
docker push "$ADMIN_CLIENT_IMAGE"

# echo "Building $WHATSAPP_IMAGE"
# docker build -t "$WHATSAPP_IMAGE" -f WhatsApp/Dockerfile ./WhatsApp

# echo "Pushing $WHATSAPP_IMAGE"
# docker push "$WHATSAPP_IMAGE"

echo "Done."
