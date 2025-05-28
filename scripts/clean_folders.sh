#!/bin/bash

# Script para eliminar recursivamente los directorios obj y bin del directorio TP
# Actualizado para un propósito más específico

echo "Iniciando limpieza de directorios obj y bin en la carpeta TP..."

# Buscar y eliminar todos los directorios bin y obj de forma más eficiente
find /Users/adibattista/Documents/GitHub/tup-25-p3/TP -type d \( -name "obj" -o -name "bin" \) | while read dir; do
  echo "Eliminando directorio: $dir"
  rm -rf "$dir"
done

# Buscar y eliminar todos los archivos *.db dentro de las carpetas tp5
echo "Eliminando archivos *.db en carpetas tp5..."
find /Users/adibattista/Documents/GitHub/tup-25-p3/TP -type d -name "tp5" | while read tp5dir; do
  find "$tp5dir" -type f -name "*.db" | while read dbfile; do
    echo "Eliminando archivo: $dbfile"
    rm -f "$dbfile"
  done
done

# Contar cuántos directorios se eliminaron
total_dirs=$(find ../TP -type d \( -name "obj" -o -name "bin" \) | wc -l)
echo "Limpieza completada! Se verificaron $total_dirs directorios obj/bin."

