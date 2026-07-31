#!/bin/bash

#I wrote this script for myself, for my own work environment; it may not work correctly for someone else.

BOLD_GREEN='\e[1;32m'
BOLD_YELLOW='\e[1;33m'
RESET='\e[0m'


read -r -p "Press Enter to start"

echo -e "${BOLD_GREEN}Adding this directory${RESET}"
git add .

read -r -p "$(echo -e "${BOLD_YELLOW}Enter the commit message: ${RESET}")" commitMessage
echo #new line

echo -e "Commit message: $commitMessage"
read -n 1 -r -p "$(echo -e "${BOLD_YELLOW}Is it correct? (y/n): ${RESET}")" isCorerct

if [ "$isCorrect" = "n" ] || [ "$isCorrect" = "N" ]; then
    read -r -p "$(echo -e "${BOLD_YELLOW}Enter new commit message: ${RESET}")" commitMessage
fi
	
echo "Committing..."
git commit -m "$commitMessage"

echo -e "${BOLD_GREEN}done.${RESET}"
echo -e "${BOLD_YELLOW}Pushing...${RESET}"
git push