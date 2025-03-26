# Hvordan bygge prosjektet
## api
Teknologi .net 9, c#, ef, auth0
Start dev: `cd api && dotnet run`

## frontend
Teknologi vue3, ts, setup, auth0
Start dev: `cd frontend && npm run dev`

Generering av api-client: `nswag openapi2tsclient /input:https://localhost:7044/openapi/v1.json /output:src/api/api-client.ts`



# Om prosjektet
En app for å registrere og konkurrere om sykkelcrux. Brukere kan opprette, dokumentere og delta i konkurranser basert på crux.

## Funksjonalitet
### Navigering
Home - Startskjerm viser kart med crux merket av
Crux - Liste med alle crux 
Crux/1 - Detaljer om et crux

### Crux-registrering
GPS-posisjon (start/sluttpunkt)
Bilder av crux
Tittel og beskrivelse
Vurdering (eks. vanskelighetsgrad)

