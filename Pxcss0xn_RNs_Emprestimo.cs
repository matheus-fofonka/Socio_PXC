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

        /// <summary>
        /// UF e CodMunicipio
        /// </summary>
        public Retorno<int> RNN04(TOEmprestimo toEmprestimo)
        {
            if (toEmprestimo.UF.FoiSetado)
            {
                toEmprestimo.UF = toEmprestimo.UF.LerConteudoOuPadrao().ToUpper();

                switch (toEmprestimo.UF)
                {
                    case "RS":
                        if (toEmprestimo.CodMunicipio.FoiSetado)
                        {
                            if (toEmprestimo.CodMunicipio.LerConteudoOuPadrao().Lenght != 7)
                            {
                                //O código do município deve ter tamanho 7.
                                return this.Infra.RetornarFalha<int>(new Mensagem(TipoMensagem.Falha_RN04_2));
                            }
                            if (!(toEmprestimo.CodMunicipio.LerConteudoOuPadrao().SubString(0, 2) == "43"))
                            {
                                //Para UF {0}, o Código do Município deve ser iniciado por { 1}.
                                return this.Infra.RetornarFalha<int>(new Mensagem(TipoMensagem.Falha_RN04_3), "RS", "43");
                            }
                        }
                        break;
                    case "SC":
                        if (toEmprestimo.CodMunicipio.FoiSetado)
                        {
                            if (toEmprestimo.CodMunicipio.LerConteudoOuPadrao().Lenght != 7)
                            {
                                //O código do município deve ter tamanho 7.
                                return this.Infra.RetornarFalha<int>(new Mensagem(TipoMensagem.Falha_RN04_2));
                            }
                            if (!(toEmprestimo.CodMunicipio.LerConteudoOuPadrao().SubString(0, 2) == "42"))
                            {
                                //Para UF {0}, o Código do Município deve ser iniciado por { 1}.
                                return this.Infra.RetornarFalha<int>(new Mensagem(TipoMensagem.Falha_RN04_3), "SC", "42");
                            }
                        }
                        break;
                    case "PR":
                        if (toEmprestimo.CodMunicipio.FoiSetado)
                        {
                            if (toEmprestimo.CodMunicipio.LerConteudoOuPadrao().Lenght != 7)
                            {
                                //O código do município deve ter tamanho 7.
                                return this.Infra.RetornarFalha<int>(new Mensagem(TipoMensagem.Falha_RN04_2));
                            }
                            if (!(toEmprestimo.CodMunicipio.LerConteudoOuPadrao().SubString(0, 2) == "41"))
                            {
                                //Para UF {0}, o Código do Município deve ser iniciado por { 1}.
                                return this.Infra.RetornarFalha<int>(new Mensagem(TipoMensagem.Falha_RN04_3), "PR", "41");
                            }
                        }
                        break;
                    default:
                        //São aceitas somente as UFs da região Sul do país.
                        return this.Infra.RetornarFalha<int>(new Mensagem(TipoMensagem.Falha_RN04_1));
                        break;
                }
            }
            return this.Infra.RetornarSucesso<int>(1, new OperacaoRealizadaMensagem());
        }

        /// <summary>
        /// Garante Existencia Emprestimo
        /// </summary>
        public Retorno<int> RNN05(TOClientePxc toClientePxc, string NomeMetodo)// “alterar”, “excluir”, “pagar” ou “cancelar”
        {
            Retorno<TOEmprestimo> retOb = Obter(toClientePxc);
            if (!retOb.Ok)
            {
                return this.Infra.RetornarFalha<int>(retOb.Mensagem);
            }
            if (!retOb.Dados.TemConteudo)
            {
                //mensagem:"Não é possível {0} o empréstimo, pois o mesmo não faz parte dos empréstimos do cliente informado."
                return this.Infra.RetornarFalha<int>(new Mensagem(TipoMensagem.Falha_RN05, NomeMetodo));
            }
            return this.Infra.RetornarSucesso<int>(1, new OperacaoRealizadaMensagem());
        }

        /// <summary>
        /// DT: Inclusao, Cancelamento, Pagto
        /// </summary>
        public Retorno<int> RNN06(TOEmprestimo toEmprestimo, string NomeMetodo)// “alterar” ou “incluir”
        {
            toEmprestimo.DtPagto = new CampoOpcional<DateTime>(null);
            toEmprestimo.DtCancelamento = new CampoOpcional<DateTime>(null);
            if (NomeMetodo == "alteracao")
            {
                toEmprestimo.DtInicio = new CampoObrigatorio<DateTime>();
            }
        }

            /// <summary>
            /// Situacao
            /// </summary>
            public Retorno<int> RNN07(TOClientePxc toClientePxc, string NomeMetodo)// “alterar”, “excluir”, “pagar” ou “cancelar”
        {
            Retorno<TOEmprestimo> retOb = Obter(toClientePxc);
            if (!retOb.Ok)
            {
                return this.Infra.RetornarFalha<int>(retOb.Mensagem);
            }

            if (NomeMetodo == "alterar" || NomeMetodo == "pagar" || NomeMetodo == "cancelar")
            {

                if (!(retOb.Dados.Situacao == EstadoSituacao.Ativo))
                {
                    //Só é possível {0} empréstimo {1}.
                    return this.Infra.RetornarFalha<int>(new Mensagem(TipoMensagem.Falha_RN07, NomeMetodo, "ativo"));
                }
            }
            if (NomeMetodo == "excluir")
            {

                if (!(retOb.Dados.Situacao == EstadoSituacao.Pago || retOb.Dados.Situacao == EstadoSituacao.Cancelado))
                {
                    //Só é possível {0} empréstimo {1}.
                    return this.Infra.RetornarFalha<int>(new Mensagem(TipoMensagem.Falha_RN07, NomeMetodo, "pago ou cancelado"));
                }
            }
            return this.Infra.RetornarSucesso<int>(1, new OperacaoRealizadaMensagem());
        }
        #endregion

    }
}
