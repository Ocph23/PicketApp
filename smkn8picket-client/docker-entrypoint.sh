#!/bin/sh
set -eu

export VITE_API_URL="${VITE_API_URL:-http://localhost:5001}"
export VITE_LOGO="${VITE_LOGO:-/smk.png}"
export VITE_NAMA_SEKOLAH="${VITE_NAMA_SEKOLAH:-SMKN 8 TIK KOTA JAYAPURA}"
export VITE_ALAMAT_SEKOLAH="${VITE_ALAMAT_SEKOLAH:-Jln. }"
export VITE_NO_TELP="${VITE_NO_TELP:-0967-}"
export VITE_EMAIL="${VITE_EMAIL:-smkn8tik@gmail.com}"
export VITE_KOTA="${VITE_KOTA:-KOTA JAYAPURA}"
export VITE_PROVINSI="${VITE_PROVINSI:-PAPUA:}"
export VITE_KODE_POS="${VITE_KODE_POS:-99111:}"

envsubst < /usr/share/nginx/html/env.template.js > /usr/share/nginx/html/config.js

exec nginx -g "daemon off;"
