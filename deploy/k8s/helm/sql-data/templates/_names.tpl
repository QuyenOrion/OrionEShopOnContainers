{{- define "sql-name" }}
{{- if .Values.info.sql.host }}
{{- printf "%s" .Values.info.sql.host }}
{{- else }}
{{- printf "%s" "sql-data" }}
{{- end }}
{{- end }}