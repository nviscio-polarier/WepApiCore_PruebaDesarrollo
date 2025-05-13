
import requests
import pytest
import urllib3
urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)

from utils_de_conexion import get_logs_token

BASE_URL = "https://localhost:7136/odata"
TOKENS_A_CANJEAR = 6
ID_PERSONA = 1392
ID_PERSONA_APROBADOR = 6969
persona_token_id_global = None
resumen_inicial = None
cantidad_aprobaciones_locales = 0
cantidad_rechazos_locales = 0

def get_resumen_tokens():
    resp = requests.get(f"{BASE_URL}/getResumenTokensPersona?idPersona={ID_PERSONA}", verify=False)
    return resp.json()


def test_solicitar_canje():
    global persona_token_id_global, resumen_inicial
    if resumen_inicial is None:
        resumen_inicial = get_resumen_tokens()

    response = requests.post(
        f"{BASE_URL}/solicitarCanje",
        params={
            "idPersona": ID_PERSONA,
            "tokensASolicitar": TOKENS_A_CANJEAR
        },
        verify=False
    )

    assert response.status_code == 200, f"Error HTTP: {response.status_code}, body: {response.text}"
    data = response.json()
    persona_token_id_global = data["IdPersonaToken"]
    assert data["TokensSolicitados"] == TOKENS_A_CANJEAR

    logs = get_logs_token()
    assert any(
        log["idPersona"] == ID_PERSONA and
        log["tipoOperacion"].upper() == "SOLICITUD_CANJE" and
        log["valorNuevo"] == TOKENS_A_CANJEAR
        for log in logs
    ), "No se encontró el log esperado de solicitud de canje"


def test_aprobar_canje():
    global persona_token_id_global, cantidad_aprobaciones_locales
    assert persona_token_id_global is not None, "No hay solicitud previa."

    params = {
        "idPersonaToken": persona_token_id_global,
        "idPersonaAprobador": ID_PERSONA_APROBADOR,
    }

    response = requests.post(f"{BASE_URL}/aprobarSolicitudCanje", params=params, verify=False)
    assert response.status_code == 200
    cantidad_aprobaciones_locales += 1

    resumen_despues = get_resumen_tokens()
    logs = get_logs_token()
    esperado_valor_nuevo = resumen_despues["TokensReales"]

    assert any(
        log["idPersona"] == ID_PERSONA and
        log["tipoOperacion"].upper() == "APROBAR_CANJE" and
        log["valorNuevo"] == esperado_valor_nuevo
        for log in logs
    ), "No se encontró el log esperado de aprobación del canje"


def test_solicitar_canje2():
    global persona_token_id_global

    response = requests.post(
        f"{BASE_URL}/solicitarCanje",
        params={
            "idPersona": ID_PERSONA,
            "tokensASolicitar": TOKENS_A_CANJEAR
        },
        verify=False
    )

    assert response.status_code == 200, f"Error HTTP: {response.status_code}, body: {response.text}"
    data = response.json()
    persona_token_id_global = data["IdPersonaToken"]
    assert data["TokensSolicitados"] == TOKENS_A_CANJEAR

    logs = get_logs_token()
    assert any(
        log["idPersona"] == ID_PERSONA and
        log["tipoOperacion"].upper() == "SOLICITUD_CANJE" and
        log["valorNuevo"] == TOKENS_A_CANJEAR
        for log in logs
    ), "No se encontró el log esperado de solicitud de canje"


def test_rechazar_canje():
    global persona_token_id_global, cantidad_rechazos_locales
    motivo_rechazo = "Rechazo test automático 12344321"
    assert persona_token_id_global is not None, "No hay solicitud previa para rechazar."

    params = {
        "idPersonaToken": persona_token_id_global,
        "idPersonaRevisor": ID_PERSONA_APROBADOR,
        "motivoRechazo": motivo_rechazo
    }

    resumen_antes = get_resumen_tokens()
    response = requests.post(f"{BASE_URL}/rechazarSolicitudCanje", params=params, verify=False)
    assert response.status_code == 200
    cantidad_rechazos_locales += 1

    resumen_despues = get_resumen_tokens()
    logs = get_logs_token()

    assert any(
        log["idPersona"] == ID_PERSONA and
        log["tipoOperacion"].upper() == "RECHAZAR_CANJE" and
        motivo_rechazo in log.get("mensaje", "")
        for log in logs
    ), f"No se encontró el log esperado con motivo '{motivo_rechazo}'"

    assert resumen_despues["TokensGanados"] >= resumen_antes["TokensGanados"], "No se reflejó la devolución en los tokens ganados"




if __name__ == "__main__":
    test_solicitar_canje()
    input("aaaa")
    test_aprobar_canje()
    test_solicitar_canje2()
    input("bbbb")
    test_rechazar_canje()
