var section = 0;
var maxsection = 5;
function questions() {
    if (section == 1) {
        document.getElementById("question").innerHTML = "Wie alt ist Günter Lauber, unser ältestes Mitglied?";
        document.getElementById("answer1").value = "78";
        document.getElementById("answer2").value = "74";
        document.getElementById("answer3").value = "81";
        console.log("Hello;");
    } else if (section == 2) {
        document.getElementById("question").innerHTML = "Wie viel wiegen alle unsere Mitglieder zusammen (Schätzfrage)?";
        document.getElementById("answer1").value = "ca. 4500 kg";
        document.getElementById("answer2").value = "ca. 4000 kg";
        document.getElementById("answer3").value = "ca. 4300 kg";
        console.log("Hello;");
    } else if (section == 3) {
        document.getElementById("question").innerHTML = "Welches Instrument wird bei uns am meisten gespielt?";
        document.getElementById("answer1").value = "Trompete";
        document.getElementById("answer2").value = "Posaune";
        document.getElementById("answer3").value = "Triangel";
        console.log("Hello;");
    } else if (section == 4) {
        document.getElementById("question").innerHTML = "Welches Jubiläum feiern wir?";
        document.getElementById("answer1").value = "40";
        document.getElementById("answer2").value = "45";
        document.getElementById("answer3").value = "50";
        console.log("Hello;");
    } else if (section == 5) {
        document.getElementById("question").innerHTML = "Was ist der Umfang des Hallwilersees?";
        document.getElementById("answer1").value = "20.281 km";
        document.getElementById("answer2").value = "19.088 km";
        document.getElementById("answer3").value = "18.989 km";
        console.log("Hello;");
    } else {

    }
}

function vorward() {
    section++;
    questions();
}

function back() {
    section--;
    questions();
}