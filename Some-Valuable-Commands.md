### How to prune everything in Docker?
```
docker system prune --all
```
### How to reboot WSL2 in Windows 10/11?
```
wsl --shutdown
wsl 
```
### How to shrink a virtual hard disk file (vhdx) of WSL2 in Windows 10/11?
```
diskpart
select vdisk file=C:\Users\U1\AppData\Local\Packages\CanonicalGroupLimited.Ubuntu20.04onWindows_79rhkp1fndgsc\LocalState\ext4.vhdx
compact vdisk
```
### How to login to Docker Hub by command line?
```
sudo docker login -u [user-name] --password-stdin [password]
```
### How to tag and push an image to Docker Hub?
```
sudo docker tag [image id] [account or namespace]/azure-pipelines-agents-debian-12.2:11072024
sudo docker push [account or namespace]/azure-pipelines-agents-debian-12.2:11072024
```
### How to run container on WSL2?
```
sudo docker build -f src/ShoppingApp.WebUI/Dockerfile . -t webuilocal
sudo docker run -p 8080:80 -t webuilocal
```