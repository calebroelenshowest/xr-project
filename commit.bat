set /p commitName=Commit message:
git add *
git commit -m "%commitName%"
git status
PAUSE