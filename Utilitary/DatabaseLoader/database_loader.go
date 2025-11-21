package main

import (
	"fmt"
	"os"
	"strings"
)

func display_help() {

	var message strings.Builder

	message.WriteString("USAGE:\n")
	message.WriteString(os.Args[0] + " [OPTIONS...]\n")
	message.WriteString("\n[OPTIONS]...\n")
	message.WriteString("-sqlite\t--sqlite-path\tPath to the SQLite database\n")

	fmt.Println(message.String())

}

func main() {

	/* CLI */
	var database_path string = ""

	for i := 0; i < len(os.Args); i++ {

		current_arg := os.Args[i]

		if strings.HasPrefix(current_arg, "-") || strings.HasPrefix(current_arg, "--") {
			if i+1 > len(current_arg) {
				break
			}

			if current_arg == "-sqlite" || current_arg == "--sqlite-path" {
				database_path = os.Args[i+1]
				break
			}

		} else {
			continue
		}

	}

	if database_path == "" {
		fmt.Println("Insuficient amount of arguments!")
		display_help()
		os.Exit(1)
	}

	var success bool = populate_db(database_path)
	if success {
		fmt.Println("Banco de dados foi inicializado com successo!")
		os.Exit(0)
	} else {
		fmt.Println("Não foi possivel inicializar o banco de dados!")
		os.Exit(1)
	}

}
