
﻿using System;
using System.Collections;
using System.Collections.Generic;
using Backoffice.Models.Petiscos;
using Backoffice.Models.GeoLocalizacao;

namespace Backoffice.Models.Backoffice
{
    public class FacadeBackoffice
    {
        
        public Dictionary<int, Petisco> Petiscos { get; set; }
		public Dictionary<int, Especialidade> Especialidades { get; set; }
		public Dictionary<int, Estabelecimento> Estabelecimentos { get; set; }

        public FacadeBackoffice()
        {
            Petiscos = new Dictionary<int, Petisco>();
            Especialidades = new Dictionary<int, Especialidade>();
            Estabelecimentos = new Dictionary<int, Estabelecimento>();
        }

        //public void adicionaEstabelecimento(String nome, String telefone, ArrayList horario, Localizacao coordenadas, ArrayList criticas) {
            
        //    Estabelecimento est = new Estabelecimento(nome, telefone, horario, coordenadas, criticas);

        //    Estabelecimentos[est.ID] = est;

        //} 

   //     public void adicionaEspecialidade(int idPetisco, List<String> caracteristicas, double preco, int idEstabelecimento, String fotografia) {

   //         Especialidade esp = new Especialidade(idPetisco, caracteristicas, preco, idEstabelecimento, fotografia);

   //         Especialidades[esp.ID] = esp;

			//Estabelecimento est;
			//if (Estabelecimentos.TryGetValue(idEstabelecimento, out est))
			//{
			//	est.AdicionaEspecialidade(esp);
			//}
            
        //}

        public void adicionaPetisco(String nome) {
            Petisco p = new Petisco{Nome=nome};
            Petiscos[p.ID] = p;
        }

        //public void atualizaEstabelecimento(int id, String nome, String telefone, ArrayList horario, Localizacao coordenadas, ArrayList criticas) {
        //    Estabelecimento est;
        //    if(Estabelecimentos.TryGetValue(id, out est)) {
        //        est.atualiza(nome, telefone, horario, coordenadas, criticas);
        //    }
        //}

        //public void atualizaEspecialidade(int id, int idPetisco, List<String> caracteristicas, double preco, int idEstabelecimento, String fotografia) {

        //    Especialidade esp;
        //    if(Especialidades.TryGetValue(id, out esp)) {
        //        esp.atualiza(idPetisco, caracteristicas, preco, idEstabelecimento, fotografia);
        //    }
        //}


        public void desactivaEstabelecimento(int id) {
            Estabelecimento est; 
            if(Estabelecimentos.TryGetValue(id, out est)) {
                est.Desactiva();
            }
        }

        public void desactivaEspecialidade(int id)
        {
            Especialidade esp;
            Especialidades.TryGetValue(id, out esp);
            esp.Desactiva();
        }
    }
}
