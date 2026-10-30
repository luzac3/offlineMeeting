#!/usr/bin/env bash

set -e

cd "$(dirname "$0")"

case "${1:-}" in
  Debug)
	npm run build
	npx electron .
	;;
  Release)
	npm run release
	npx electron-forge import
	npm run make
	;;
  *)
	echo "Usage: $0 Debug|Release" >&2
	exit 1
	;;
esac
