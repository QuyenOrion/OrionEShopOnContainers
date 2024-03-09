{{- define "redis-name" }}
{{- if .Values.info.redis.host }}
{{- printf "%s" .Values.info.redis.host }}
{{- else }}
{{- printf "%s" "redis-server" }}
{{- end }}
{{- end }}