   ___                 _                     _            
  / __\ ___  _ __ ___ | |__   ___ _ __ /\/\ (_)_ __   ___ 
 /__\/// _ \| '_ ` _ \| '_ \ / _ \ '__/    \| | '_ \ / _ \
/ \/  \ (_) | | | | | | |_) |  __/ | / /\/\ \ | | | |  __/
\_____/\___/|_| |_| |_|_.__/ \___|_| \/    \/_|_| |_|\___|



@author:	Pascal Reminy, Jérémie Ferreira
@description:	@see GameDocBomberMine.docx
@feaures:
	-paramétrage client/serveur indépendant du build
	-2 joueurs déplaçables
	-Bombes et mines implémentées sans composante réseau encore et sans pouvoir être directement utilisées par le joueur (elles sont placées dans le niveau pour l'instant)

@warning:	L'alpha que nous avons envoyé a un problème dans sa configuration réseau, nous avons résolu ces problèmes après le rendu et pour ne pas tout renvoyer, nous vous envoyons une 2nde build "CorrectedBuild.exe". Les modifications opérées sont les suivantes :
	-suppression des NetworkViews des joueurs
	-sur le NetworkView de l'empty Network : StateSync passé à Unreliable et Oberved passé à None

@QuickNote: les déplacements se font avec ZQSD et la direction est reprise de la vue du joueur.






