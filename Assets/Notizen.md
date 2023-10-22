# Notizen

### Drohne
* Rigidbody
    * CollisionDetection muss Continous sein
        * Drohne glitcht sonst z.B. durch Planes
    * Mass basierend auf eigener Drohne
    * Drag & Angular Drag erhöht
        * in der Realität wirkt das Drehmoment / die Rotationskraft der Rotoren (die immer mindestens leicht drehen), den Bewegungen der Drohen entgegen und bremsen Bewegungen ab
    * Durch Rigidbody können einzelen Gameobjects nicht mehr auf Kollisdion geprüft werden
        * erkennung über collision.getContact(index) -> liefert 0: eigenes Objekt, 1: getroffenes Objekt
        * kann unterscheiden was getroffen wurde: welcher Rotor (tatsächliche Rotorblätter) oder Frame selbst 
            * Drohne kann dadurch erkennen von welcher Seite es gegen z.B. Wände geflogen ist
            * Umgebung wird zurückgesetzt wenn das passiert
        * **Alles was Zustand & Kollisionen der Drohne betrifft, ist im DroneHandler-Script (auch GroundCheck Zusammenfassung)** 
    * Drohne muss zwische CollisionEnter & TriggerEnter unterscheiden:
        * Collision führt zu "Absturz" bzw. zurücksetzen der Umgebung (Wände & Torrahmen)
        * Trigger für durchfliegen von Objekten (HittableBlock & Gates) um Punkte fürs Training zu erhalten
    * damit Tore erkennen können das Drohne durchgeflogen ist, haben Motoren einen istTrigger Collider im Zentrum
         
* Motoren
    * **Eigenschaften**: 
        * Position
        * Geschwindigkeit
        * isGrounded
    * MotorBase erzeigt extra Abstand zu Boden, da Stangen von Drohne auf Kollisionen mit Ground reagieren
        * GroundCheck  in Motoren
    * hat forcepoint an dem Kraft auf Rigidbody der Drohne ausgeübt wird
        * Kraft eine Motors wird damit simuliert
    * hat eine Drehrichtung CW oder CCW, basierend auf der Position an der Drohne
    * Rotoren drehen basierend auf motorSpeed für visuelles Feedback (in RPM?)

### Trainingsumgebung 
* Ablauf Trainingsumgebung als PAP
* sollte zu Beginn des Trainings (insgesamt) schon alle Anpassungsmöglichkeiten & Funktionen besitzen um Umgebung durchs gesamte Training Konsistent zu halten (kann KI beeinflussen)
    * Größe verstellbar
    * Anzahl & Art der Spawnables einstellbar
        * **Wichtig:** Anzahl versuche für platzieren der Objekte (gelernt bei vorherigem Projekt) (kann sonst zu Endlosschleife führen)
    * Transparenz der Objekte die gespawnt werden (später fürs Training relevant)
* Steuert das gesamte erzeugen und zurücksetzen (auch Drohne)
    * Wichtig hier: Rigidbody der Drohne muss "zurückgesetzt" werden
    * Random rotation bei start (Overfitting)
* **ALLE Positionen sind basierend auf Center-Objekt**
    * Wichtig da es mehrere Trainingsumgebungen geben wird und diese an verschiedenen Positionen sind
* Spawnables
    * **Eigenschaften**: 
        * Typ
        * Punktzahl
        * Materialtransparenz der durch die Drohen zu treffenden Objekte, durch Umgebung geregelt
        * SphereCollider mit isTrigger für placement
    * NONE
        * Keine Objekte (wird zu Beginn des Ttainings benötigt)
    * HITTABLEBLOCK
        * simpler Block gegen den geflogen wird, um Punkte zu erhalten
    * SMALLGATE & BIGGATE
        * Tore durch die geflogen wird
        * Tordurchgang in Farbe der HittableBlocks
        * Logik für prüfen ob Drohne durch Tor geflogen ist: **PAP-Zeichnen**
            * Tor prüft bei Kollision im Durchgang ob Gameobject Motor-Skript besitzt
            * speichert Position getroffener Motoren in Hashset (daurch keine Kontrolle auf doppelte Motoren)
            * wenn 4 Einträge in Liste => Tor cleared
            * liste 4 sekunden nach letzer Motor kollision zurückgesetz, verhindert permanenten half-cleared Zustand
                * interessant: nicht über Update (hohe kosten), sondern über Time.time() (gloable zeit seit Start)
                
### Steuerung
* Steuerung erklären mit Bilder (wie steuert eine Drohne?)
* Besonderheiten: 
    * Yaw
    * CalculateRatesValue (Rates über Funktion f(x))

### KI-Basis
* Wissen:
    * isgrounded von DroneHandler = OR Zusammemfassung von allen Motoren
    * Zustand der einzelnen Rotoren & Frame
    * relistisches Wissen der Piloten
        * Rotation der Drohne
        * Beschleunigung der Drohne in alle Richtungen
    * Kamera-Sensor
* prüft auf Zustände der Drohne (Frame & Rotoren in DroneHandler)

* Manuelle Steuerung über SelfPlay / Heuristic
    * nutzt neues Input-System

## Planung Training

* drawio