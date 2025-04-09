#!/bin/bash

# Build for Linux
echo "Building for Linux..."
/usr/bin/unity-editor \
  -batchmode \
  -nographics \
  -silent-crashes \
  -logFile BuildLog.txt \
  -projectPath "$(pwd)/PlatformerGame" \
  -buildTarget Linux64 \
  -quit \
  -executeMethod BuildScript.BuildLinux

echo "Build process completed!"
