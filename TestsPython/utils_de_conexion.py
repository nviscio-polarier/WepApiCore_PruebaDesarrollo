import pyodbc
from datetime import datetime
import requests

conn = pyodbc.connect(
"DRIVER={ODBC Driver 17 for SQL Server};"
"SERVER=81.46.202.72,1455\\TEST;"  
"DATABASE=bdERP_alex;"
"UID=hackathon;"                   
"PWD=Hack1793"
)

BASE_URL = "https://localhost:7136/odata"


def limpiar_solicitudes_pendientes(id_persona, conn_string):
    try:
        conn = pyodbc.connect(conn_string)
        cursor = conn.cursor()
        evento_en_revision = 5

        # Eliminar solicitudes relacionadas
        cursor.execute("""
            DELETE FROM MyRealBonus.tblSolicitudesTokens 
            WHERE idPersonaToken IN (
                SELECT idPersonaToken 
                FROM MyRealBonus.tblPersonaTokens 
                WHERE idPersona = ? AND idTipoEventoToken = ?
            )
        """, id_persona, evento_en_revision)

        # Eliminar registros de tokens en revisión
        cursor.execute("""
            DELETE FROM MyRealBonus.tblPersonaTokens 
            WHERE idPersona = ? AND idTipoEventoToken = ?
        """, id_persona, evento_en_revision)

        conn.commit()
        print(f"Solicitudes pendientes de {id_persona} eliminadas.")
    except pyodbc.Error as e:
        print("Error al limpiar:", e)
    finally:
        conn.close()


def insertar_token(id_persona, tokens, id_lavanderia, conn_string):
    try:
        conn = pyodbc.connect(conn_string)
        cursor = conn.cursor()

        fecha = datetime.now()

        cursor.execute("""
            INSERT INTO tblPersonaTokens (
                idPersona, fecha, tokens, isCanje,
                idTipoEventoToken, idLavanderia
            ) VALUES (?, ?, ?, ?, ?, ?)
        """, id_persona, fecha, tokens, False, 1, id_lavanderia)

        conn.commit()
        print("ok")

    except pyodbc.Error as e:
        print("No va ", e)

    finally:
        conn.close()

def get_logs_token():
    resp = requests.get(f"{BASE_URL}/getLogsToken?$orderby=fechaHora desc&$top=10", verify=False)
    assert resp.status_code == 200
    return resp.json()