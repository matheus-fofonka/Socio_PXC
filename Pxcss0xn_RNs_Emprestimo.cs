using System;
using System.Collections.Generic;
using System.Text;
using Bergs.Pxc.Pxcbtoxn;
using System.Runtime.InteropServices;
using Bergs.Pwx.Pwxoiexn.MM3;

namespace Bergs.Pxc.Pxcss0xn
{
    internal class RNsEmprestimo
    {
        #region RN_Emprestimo
        /// <summary>
        /// valida agencia
        /// </summary>
        public Retorno<int> RNN01(TOClientePxc toClientePxc)
        {
            //using Pxcsclxn
            RNClientePxc rnCli = this.Infra.InstanciarRN<RNClientePxc>();
            Retorno<TOClientePxc> retOb = rnCli.Obter(toClientePxc);
            if (!retOb.Ok)
            {
                return this.Infra.RetornarFalha<int>(retOb.Mensagem);
            }
            if (!retOb.Dados.TemConteudo)
            {
                //mensagem:"Cliente não encontrado."
                return this.Infra.RetornarFalha<int>(new Mensagem(TipoMensagem.Falha_RN01));
            }
            toClientePxc.agencia = retOb.Dados.agencia.LerConteudoOuPadrao();
        }

        /// <summary>
        /// valida Data Inicio
        /// </summary>
        public Retorno<int> RNN02(TOClientePxc toClientePxc)
        {
            if (toClientePxc.DtInclusao.FoiSetado)
            {
                if (!(toClientePxc.DtInclusao.LerConteudoOuPadrao().Date <= DateTime.Now.Date))
                {
                    //“Data de Inclusão, se informada, deve ser menor ou igual a data atual."
                    return this.Infra.RetornarFalha<int>(new Mensagem(TipoMensagem.Falha_RN02));
                }
                return this.Infra.RetornarSucesso<int>(1, new OperacaoRealizadaMensagem());
            }
            else
            {
                toClientePxc.DtInclusao = DateTime.Now.Date;
            }
            return this.Infra.RetornarSucesso<int>(1, new OperacaoRealizadaMensagem());
        }

        /// <summary>
        /// valor_emp taxa
        /// </summary>
        public Retorno<int> RNN03(TOClientePxc toClientePxc)
        {
            if (toClientePxc.ValorEmp.FoiSetado)
            {
                if (!(toClientePxc.ValorEmp.LerConteudoOuPadrao() >= 1000.00 && toClientePxc.ValorEmp.LerConteudoOuPadrao() <= 1000000.00))
                {
                    //O Valor do Empréstimo deve estar compreendido entre R$ 1.000,00 e R$ 1.000.000,00, inclusive.
                    return this.Infra.RetornarFalha<int>(new Mensagem(TipoMensagem.Falha_RN03_1));
                }
            }
            //O campo TAXA, se informado, deve ser positivo e menor que 10 %.Caso não respeite essa regra, o sistema interrompe o
            //processamento e retorna mensagem de falha: “A Taxa do Empréstimo deve ser positiva e menor que 10 %.”;
            if (toClientePxc.Taxa.FoiSetado)
            {
                if (!(toClientePxc.Taxa.LerConteudoOuPadrao() > 0 && toClientePxc.ValorEmp.LerConteudoOuPadrao() < 10))
                {
                    //A Taxa do Empréstimo deve ser positiva e menor que 10%.
                    return this.Infra.RetornarFalha<int>(new Mensagem(TipoMensagem.Falha_RN03_2));
                }
                
            }
            return this.Infra.RetornarSucesso<int>(1, new OperacaoRealizadaMensagem());
        }


        #endregion
    }
}
