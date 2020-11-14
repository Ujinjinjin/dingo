linux_exe:
	make clean_up
	mkdir ~/temp ~/temp/dingo ~/temp/dingo/DEBIAN ~/temp/dingo/usr ~/temp/dingo/usr/share ~/temp/dingo/usr/share/dingo
	make control_content -s > ~/temp/dingo/DEBIAN/control
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
	rm -rf bin obj ~/temp dingo.deb