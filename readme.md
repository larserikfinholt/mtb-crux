# Formål
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

### Konkurranser
Opprette konkurranser med flere crux
Brukere kan registrere seg (anonymt/telefon)
Krysse av for gjennomførte crux og registrere poeng

### Brukerinteraksjon
Liste over tilgjengelige konkurranser
Vise gjennomførte crux med poengscore
Filtrering og søk etter crux

## Teknologi
Frontend: Vue 3 (Composition API, TypeScript, TailwindCSS, leaflet)
Backend: .NET 9, Entity Framework Core, REST API
Database: PostgreSQL / SQL Server
Autentisering: Anonym registrering eller telefonnummer

### Tips
Genereate ts api client `nswag openapi2tsclient /input:https://localhost:7044/openapi/v1.json /output:src/api/api-client.ts
`
