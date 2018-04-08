//
//  Program.cs
//
//  Author:
//       Jeffryn Salgado <jeffrynsl@hotmail.es>
//
//  Copyright (c) 2018 Jeffryn Salgado
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.IO;
using System.Diagnostics;

namespace iofile
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			string Command;
			string[] Commands;
			const char C = ' ';
			DateTime Inicio, Fin;
			do{
				Console.Title = "IOFile v0.1";

				Console.ForegroundColor = ConsoleColor.Green;

				Console.Write ("root@{0}:~#",Environment.MachineName); Console.ResetColor();
				Command = Console.ReadLine ();

				Commands = Command.Split (C);

				switch(Commands[0]){

				case "clear":{
						Console.Clear();
					}
					break;
				case "cd":{
						try{
							Directory.SetCurrentDirectory (Commands[1]);
						}catch (DirectoryNotFoundException Ex){
							Console.WriteLine(Ex.Message);
						}catch (FileNotFoundException Ex){
							Console.WriteLine(Ex.Message);
						}catch (UnauthorizedAccessException Ex){
							Console.WriteLine(Ex.Message);
						}
					}
					break;
				case "ls":{
						try{
							foreach(string D in Directory.GetDirectories (Directory.GetCurrentDirectory())){
								Console.WriteLine (D);
							}
							foreach (string F in Directory.GetFiles(Directory.GetCurrentDirectory())) {
								Console.WriteLine (F);
							}
						}catch (DirectoryNotFoundException Ex){
							Console.WriteLine(Ex.Message);
						}catch (FileNotFoundException Ex){
							Console.WriteLine(Ex.Message);
						}
					}
					break;
				case "pwd":{
						try{
							Console.WriteLine(Directory.GetCurrentDirectory());
						}catch(DirectoryNotFoundException Ex){
							Console.WriteLine(Ex.Message);
						}
					}
					break;
				case "disk":{
						
						foreach(DriveInfo D in DriveInfo.GetDrives()){
							if(D.IsReady){
								Console.WriteLine("Nombre: {0}",D.Name);
								Console.WriteLine("Etiqueta: {0}",D.VolumeLabel);
								Console.WriteLine("Tipo de Volumen: {0}",D.DriveType);
								Console.WriteLine("Formato de Volumen: {0}",D.DriveFormat);
								Console.WriteLine("Espacio Total: {0:F}",Gb(D.TotalSize));
								Console.WriteLine("Espacio Libre: {0:F}",Gb(D.TotalFreeSpace));
								Console.WriteLine("Espacio Usado: {0:F}",Gb(D.TotalSize - D.AvailableFreeSpace));
								if(Porcentaje(D.TotalSize - D.AvailableFreeSpace,D.TotalSize)>=85){
									Console.ForegroundColor = ConsoleColor.Red;
									Console.WriteLine("Porcentaje de Uso: {0:F}%",Porcentaje(D.TotalSize - D.AvailableFreeSpace,D.TotalSize));
								}else{
									Console.ForegroundColor = ConsoleColor.Blue;
									Console.WriteLine("Porcentaje de Uso: {0:F}%",Porcentaje(D.TotalSize - D.AvailableFreeSpace,D.TotalSize));
								}
								if(Porcentaje(D.AvailableFreeSpace,D.TotalSize)>=50){
									Console.ForegroundColor = ConsoleColor.Blue;
									Console.WriteLine("Porcentaje Libre: {0:F}%",Porcentaje(D.AvailableFreeSpace,D.TotalSize));
								}else{
									Console.ForegroundColor = ConsoleColor.Red;
									Console.WriteLine("Porcentaje Libre: {0:F}%",Porcentaje(D.AvailableFreeSpace,D.TotalSize));
								}
								Console.ResetColor();
							}
						}
					}
					break;
				case "cp":{
						try{
							Inicio = DateTime.Now.ToLocalTime();
							File.Copy(Commands[1],Commands[2]);
							Fin = DateTime.Now.ToLocalTime();
							Console.WriteLine("Completado!!! \a");
							Console.WriteLine("Tiempo Inicial: {0}\nTiempo Final: {1}\nDuracion: {2}",Inicio,Fin,Fin-Inicio);
						}catch(DirectoryNotFoundException Ex){
							Console.WriteLine(Ex.Message);
						}catch(FileNotFoundException Ex){
							Console.WriteLine(Ex.Message);
						}catch(UnauthorizedAccessException Ex){
							Console.WriteLine(Ex.Message);
						}
					}
					break;
				case "mv":{
						try{
							Inicio = DateTime.Now.ToLocalTime();
							Directory.Move(Commands[1],Commands[2]);
							Fin = DateTime.Now.ToLocalTime();
							Console.WriteLine("Completado!!! \a");
							Console.WriteLine("Tiempo Inicial: {0}\nTiempo Final: {1}\nDuracion: {2}",Inicio,Fin,Fin-Inicio);
						}catch(DirectoryNotFoundException Ex){
							Console.WriteLine(Ex.Message);
						}catch(FileNotFoundException Ex){
							Console.WriteLine(Ex.Message);
						}catch (IOException Ex){
							Console.WriteLine(Ex.Message);
						}
					}
					break;
				case "ps":{
						try{
							foreach(Process P in Process.GetProcesses()){
								Console.WriteLine(P.ProcessName);
							}
						}catch (Exception Ex){
							Console.WriteLine(Ex.Message);
						}
					}
					break;
				case "dirlist":{
						Paths(Commands[1],Commands[2]);
					}
					break;
				case "del":{
						Del(Commands[1],Commands[2]);
					}
					break;
				case "find":{
						Find(Commands[1]);
					}break;
				case "exit":{
						goto Salir;
					};
				}
			}while(Commands[0]!="exit"); Salir:;
			//Console.ReadKey (true);
		}
		public static float Kb(float value){
			return value / 1024;
		}
		public static float Mb(float value){
			return Kb (value) / 1024;
		}
		public static float Gb(float value){
			return Mb (value) / 1024;
		}
		public static double Porcentaje(double value,double TotalValue){
			return (value * 100) / TotalValue;
		}
		public static void Paths(string sourcepath, string destinypath){
			try{
				StreamWriter Wr = null;
				//StreamReader Read;
				DirectoryInfo DirInf =  new DirectoryInfo(sourcepath); 
				foreach(DirectoryInfo Dir in DirInf.GetDirectories()){
					if(File.Exists(destinypath)){
						Wr = File.AppendText(destinypath);
						Wr.WriteLine(Dir.Name);
					}else{
						Wr = File.CreateText(destinypath);
						Wr.WriteLine(Dir.Name);
					}
					Wr.Close();
					Paths(Dir.FullName,destinypath);
				}
			}catch (DirectoryNotFoundException Ex){
				Console.WriteLine(Ex.Message);
			}catch (FileNotFoundException Ex){
				Console.WriteLine(Ex.Message);
			}catch (IOException Ex){
				Console.WriteLine(Ex.Message);
			}
		}
		public static void Del(string sourcepath,string file){
			try{
				DirectoryInfo DirInf= new DirectoryInfo(sourcepath);
				if(File.Exists(DirInf.FullName+"/"+file)){
					File.Delete(DirInf.FullName+"/"+file);
					Console.WriteLine("Archivo Eliminado Con Exito!!!");
				}else{
					Console.WriteLine("No se Encontro El Archivo!!! {0}",DirInf.FullName+"/"+file);
				}
				foreach(DirectoryInfo Dir in DirInf.GetDirectories()){
					if(File.Exists(Dir.FullName+"/"+file)){
						File.Delete(Dir.FullName+"/"+file);
						Console.WriteLine("Archivo Eliminado Con Exito!!!");
					}else{
						Console.WriteLine("No se Encontro El Archivo!!! {0}",Dir.FullName+"/"+file);
					}
					Del(Dir.FullName,file);
				}
			}catch (DirectoryNotFoundException Ex){
				Console.WriteLine (Ex.Message);
			}catch (FileNotFoundException Ex){
				Console.WriteLine (Ex.Message);
			}catch(UnauthorizedAccessException Ex){
				Console.WriteLine (Ex.Message);
			}
		}
		public static void Find(string Arch){
			try{
				DirectoryInfo DirInf= new DirectoryInfo(Directory.GetCurrentDirectory());

				if(File.Exists(DirInf.FullName+Arch)){
					Console.WriteLine(DirInf.FullName+Arch);
				}
				foreach(DirectoryInfo D in DirInf.GetDirectories()){
					if(File.Exists(DirInf.FullName+Arch)){
						Console.WriteLine(DirInf.FullName+Arch);
					}
					DirInf = new DirectoryInfo(D.FullName);
					Find(Arch);
				}
			}catch(DirectoryNotFoundException Ex){
				Console.WriteLine (Ex.Message);
			}catch(FileNotFoundException Ex){
				Console.WriteLine (Ex.Message);
			}catch(UnauthorizedAccessException Ex){
				Console.WriteLine (Ex.Message);
			}catch(Exception Ex){
				Console.WriteLine (Ex.Message);
			}
		}
	}
}