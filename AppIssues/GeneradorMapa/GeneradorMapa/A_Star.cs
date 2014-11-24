﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneradorMapa
{
    public class A_Star: Method
    {
        private List<TrayectoriaParcial> A_;
        private List<TrayectoriaParcial> C_;

        public A_Star(Robot r)
            : base(r)
        {
            TrayectoriaParcial aux;
            aux = new TrayectoriaParcial();


            aux.append(toNodo(robot_.get_pos()), "-");

            A_ = new List<TrayectoriaParcial> ();
            C_ = new List<TrayectoriaParcial> ();

            A_.Add(aux);
        }
        public override void run() 
        {
            TrayectoriaParcial aux =null;

            while (A_.Count != 0)
            {
                aux = A_[0];
                //Analizar primera trayectoria, si termina en objetivo terminar.
                if (aux.get_Nfinal() == toNodo(robot_.get_meta().x, robot_.get_meta().y))
                {
                    //Finalizar
                    A_.Clear();
                }
                else
                {
                    //De lo contrario: Borrarla e incluirla en la lista cerrada: void incluir_cerrada(Trayectoria)
                    A_.Remove(aux);
                    incluir_cerrada(aux);
                    //Ramificar la trayectoria eliminada de abierta
                    ramificar(aux);
                    //Añadir las nuevas trayectorias a la lista abierta
                    //Ordenar la abierta y eliminar con el mismo final tanto en abierta como en cerrada
                    var sortedA =
                         from w in A_
                         orderby w.get_coste()
                         select w;
                    A_.Clear();
                    foreach (var i in sortedA)
                    {
                        A_.Add(i);
                    }
                }
                }


        }
        void incluir_cerrada(TrayectoriaParcial t)
        {
            for (int i = 0; i < C_.Count; i++)
            {
                if (C_[i].get_Nfinal() == t.get_Nfinal())
                {   //Si hay alguna con el mismo final eliminar la de mayor coste
                    if (C_[i].get_coste() > t.get_coste())
                    {
                        C_.RemoveAt(i);
                        C_.Add(t);
                        break;
                    }
                    else
                    {
                        break;
                    }

                }
            }  
        }
        void ramificar(TrayectoriaParcial t)
        {
            List<TrayectoriaParcial> nuevas = new List<TrayectoriaParcial>();

            //Mover al bicho hasta nodo en común, bla bla pero por ahora con teletransporte :D

            robot_.set_pos(toPosicion(t.get_Nfinal()));
            robot_.actualizarSensores();

            if (robot_.get_sensores()[(int)Direcciones.NORTE] == 0)
            {
                nuevas.Add(new TrayectoriaParcial(t));
                nuevas[nuevas.Count - 1].append(toNodo(robot_.get_pos()) - 1, "N");
            }
            if (robot_.get_sensores()[(int)Direcciones.SUR] == 0)
            {
                nuevas.Add(new TrayectoriaParcial(t));
                nuevas[nuevas.Count - 1].append(toNodo(robot_.get_pos()) + 1, "S");
            }
            if (robot_.get_sensores()[(int)Direcciones.ESTE] == 0)
            {
                nuevas.Add(new TrayectoriaParcial(t)); 
                nuevas[nuevas.Count - 1].append(toNodo(robot_.get_pos()) + robot_.get_parent().get_tab().get_columns(), "E");
            }
            if (robot_.get_sensores()[(int)Direcciones.OESTE] == 0)
            {
                nuevas.Add(new TrayectoriaParcial(t));
                nuevas[nuevas.Count - 1].append(toNodo(robot_.get_pos()) - robot_.get_parent().get_tab().get_columns(), "O");
            }

            nuevas.ForEach(delegate(TrayectoriaParcial aux)
            {
                bool ok = true;
                bool insertar = false;
                //Si esta en la cerrada entonces a borrala!
                for (int i = 0; i < C_.Count; i++)
                {
                    if (C_[i].get_Nfinal() == aux.get_Nfinal())
                    {
                        ok = false;
                        break;
                    }
                }
                if (ok)
                {
                    insertar = true;
                    //Si no esta en la cerrada, ver si esta en la abierta
                    for (int i = 0; i < A_.Count; i++)
                    {
                        if (A_[i].get_Nfinal() == aux.get_Nfinal())
                        {
                            if (A_[i].get_coste() > aux.get_coste())                            
                                A_.RemoveAt(i);                          
                            else                           
                                insertar = false;
                            break;
                        }
                    }
                    if (insertar)
                    {
                        A_.Add(aux);
                    }
                }
            });

        }


    }
}
