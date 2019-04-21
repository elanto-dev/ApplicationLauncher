# Programmi kirjeldus

P.S. Hetkeseisul programmi toorik on juba käes. Tuleb parandada mõned bugid ning lisada paar meetodid. 
Programmis kasutasin fileWatcherService just selle ülesande täitmiseks.
Praegu programm loeb START.ini failist parameetrid ning kirjutab neid logisse parameetrite jaoks moodustatud .txt faili.
Selleks, et fail oleks õigeks määratud, peavad olema mõlemad sektsioonid (Parameters ja Bottles), vajalikud parameetrid 
(ees- ja perekonnanimi, sünnipäev, proovi võtmise aeg ja ssn).
Vigase faili korral service jättab logi ning kustutab vigase faili ära. Veidi veel uurin ja lisan võimaluse, 
et vigaste failide sisu koliks ümber logis olevatesse vigaste failide hulka. Praegu programm sinult moodustab uue faili kuid sisu kustutakse koos vana vigase failiga.
Tuleb tegeleda praegu DateTime parameetritega, sest praegu neid salvestatakse lihtsalt otse stringina vales formaadis.
Lisaks sooviks teada kust võtta StudyId, sest kirjelduses oli see ühes kohas SampleNumber, aga näites SampleNumber ja StudyId numbrit ei kattunud kokku.

# All on toodud programmi käivitamine

Selleks, et service läks käima peale allalaadimist peavad olema täidetud allolevad käsud käsurealt # RUN AS ADMINISTRATOR

```
cd C:\Windows\Microsoft.NET\Framework\v4.0.30319
InstallUtil.exe <full path> (e.g. D:\ORNet\Launcher\LauncherService\bin\Debug\LauncherService.exe)
```

Selleks, et määrata kust hakkavad tulema START.ini failid, tuleb konfiguratsiooni failis **App.config** muuta välja 
**MonitoringPath** Teile sobivaks teeks. Lisaks tuleks määrata ka teed logi failide jaoks.

Peale seda:
---
Win + R   --->  services.msc  ---> leida ORNet.Launcher   ---> käivitada
---

