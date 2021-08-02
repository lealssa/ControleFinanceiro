import csv
import requests
import json
from datetime import datetime
from contextlib import closing

# Cadastro dos fundos
# Metadados
# http://dados.cvm.gov.br/dados/FI/CAD/META/meta_cad_fi.txt
CAD_FI = "http://dados.cvm.gov.br/dados/FI/CAD/DADOS/cad_fi.csv"
CAD_FI_FILE = "/root/projetos-dotnet/ControleFinanceiro/cad_fi.csv"
# Informe Diario
# Metadados
# http://dados.cvm.gov.br/dados/FI/DOC/INF_DIARIO/META/meta_inf_diario_fi.txt
# http://dados.cvm.gov.br/dados/FI/DOC/INF_DIARIO/DADOS/inf_diario_fi_202107.csv
DIARIO_MESANO = datetime.now().strftime("%Y%m") #Ex. 202108
#DIARIO_FI = f"http://dados.cvm.gov.br/dados/FI/DOC/INF_DIARIO/DADOS/inf_diario_fi_{DIARIO_MESANO}.csv"  
DIARIO_FI = f"http://dados.cvm.gov.br/dados/FI/DOC/INF_DIARIO/DADOS/inf_diario_fi_202107.csv"  

def get_database():
    from pymongo import MongoClient
    import pymongo

    # Provide the mongodb atlas url to connect python to mongodb using pymongo
    CONNECTION_STRING = "mongodb://root:root@localhost:27017"

    # Create a connection using MongoClient. You can import MongoClient or use pymongo.MongoClient
    from pymongo import MongoClient
    client = MongoClient(CONNECTION_STRING)

    # Create the database for our example (we will use the same database throughout the tutorial
    return client['controle_financeiro']


def read_csv_from_url_antigo(csv_file_url):
    dict_list = []    
    with requests.Session() as s:
        download = s.get(csv_file_url)

        if not download.ok:
            raise Exception(f'Erro baixando csv ({download.reason})')        

        #print(download.content)

        csv_data = csv.DictReader(download.content, delimiter=';')

        for row in csv_data:
            print(row)
            row['_id'] = row['CNPJ_FUNDO']            
            dict_list.append(row)        

    return dict_list

def read_csv_from_url(csv_file_url):
    dict_list = []
    with closing(requests.get(csv_file_url, stream=True)) as r:
        f = (line.decode('utf-8') for line in r.iter_lines())
        reader = csv.DictReader(f, delimiter=';')
        for row in reader:
            row['_id'] = f"{row['DT_COMPTC']}{row['CNPJ_FUNDO']}"            
            dict_list.append(row)  
    return dict_list            


def csv_file_to_dict(csv_file_path):
    dict_list = []
    with open(csv_file_path, 'r', encoding='latin1') as f:
        csv_data = csv.DictReader(f, delimiter=';')
        for row in csv_data:
            row['_id'] = row['CNPJ_FUNDO']            
            dict_list.append(row)

    return dict_list  

fi_diario = read_csv_from_url(DIARIO_FI)

print(len(fi_diario))

#mongo_db = get_database()

#for fundo in fi_diario:
#    mongo_db['cvm_diario_di'].update({'_id': f"{fundo['DT_COMPTC']}{fundo['CNPJ_FUNDO']}" }, fundo, upsert=True)

#dict_data = csv_file_to_dict(CAD_FI_FILE)

#for fundo in dict_data:
#    mongo_db['cvm_fundos_di'].update({'_id': fundo['CNPJ_FUNDO'] }, fundo, upsert=True)