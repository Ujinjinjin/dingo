coverage:
	dotnet test -c release --collect "XPlat Code Coverage" --results-directory ./coverage/results
	dotnet reportgenerator -reports:coverage/results/**/*.xml -targetdir:coverage -reporttypes:"Html_Dark;Cobertura;lcov"
