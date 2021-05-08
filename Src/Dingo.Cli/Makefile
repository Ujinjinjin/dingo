linux_exe:
	make full_clean_up
	mkdir ~/temp ~/temp/dingo ~/temp/dingo/DEBIAN ~/temp/dingo/usr ~/temp/dingo/usr/share ~/temp/dingo/usr/share/dingo
	make control_content -s > ~/temp/dingo/DEBIAN/control
	dotnet test ../Dingo.UnitTests/Dingo.UnitTests.csproj -c release
	dotnet publish -c release --self-contained --runtime linux-x64 -o bin/linux
	cp -r bin/linux/* ~/temp/dingo/usr/share/dingo
	dpkg-deb --build ~/temp/dingo
	cp ~/temp/dingo.deb .
	make clean_up

control_content:
	python3 ../../tools/control.py\
		-p dingo\
		-v 1.0.1\
		-s 1024\
		-d "Local build of dingo"

clean_up:
	rm -rf bin obj ~/temp

linux_install:
	sudo dpkg --remove dingo
	make linux_exe -s
	sudo dpkg --install dingo.deb
	make full_clean_up -s

full_clean_up:
	rm -rf bin obj ~/temp dingo.deb

mssql_start:
	sudo docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Qwer1234_" \
       -p 1433:1433 --name sql1 -h sql1 \
       -d mcr.microsoft.com/mssql/server:2019-latest

mssql_stop:
	sudo docker stop sql1
	sudo docker rm sql1